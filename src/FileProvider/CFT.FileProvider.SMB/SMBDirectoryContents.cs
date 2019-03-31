using CFT.FileProvider.Abstractions;
using SharpCifs.Smb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFT.FileProvider.SMB
{
    internal class SMBDirectoryContents : ICFTDirectoryContents
    {
        SmbFile _directory;

        public SMBDirectoryContents(SmbFile directory)
        {
            if (!directory.IsDirectory())
            {
                throw new ArgumentException($"'{directory.GetName()}' не является каталогом.");
            }
            if (!directory.Exists())
            {
                throw new ArgumentException($"Каталог '{directory.GetName()}' не существует.");
            }

            _directory = directory;
        }

        public bool Exists => _directory.Exists();

        public IEnumerator<ICFTFileInfo> GetEnumerator()
        {
            return _directory
                .ListFiles()
                .Select(smbFile => new SMBFileInfo(smbFile))
                .Cast<ICFTFileInfo>()
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _directory
                .ListFiles()
                .Select(smbFile => new SMBFileInfo(smbFile))
                .Cast<ICFTFileInfo>()
                .GetEnumerator();
        }
    }
}
