using CFT.FileProvider.Abstractions;
using SharpCifs.Smb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CFT.FileProvider.SMB
{
    internal class SMBFileInfo : ICFTFileInfo
    {
        SmbFile _file;

        public SMBFileInfo(string path)
            : this(new SmbFile(path))
        {
        }

        public SMBFileInfo(SmbFile file)
        {
            //if (!file.IsFile())
            //{
            //    throw new ArgumentException($"'{file.GetName()}' не является файлом.");
            //}
            //if (!file.Exists())
            //{
            //    throw new ArgumentException($"Файл '{file.GetName()}' не существует.");
            //}

            _file = file;
        }

        public bool Exists => _file.Exists();

        public long Length => _file.Length();

        public string PhysicalPath => _file.GetPath();

        public string Name => _file.GetName();

        public DateTimeOffset LastModified => new DateTimeOffset(_file.GetLocalLastModified());

        public bool IsDirectory => _file.IsDirectory();

        public Stream CreateReadStream()
        {
            var stream = _file.GetInputStream();
            return stream;
        }

        public Stream CreateWriteStream()
        {
            var stream = _file.GetOutputStream();
            return stream;
        }

        public void Delete()
        {
            _file.Delete();
        }

        public async Task RenameAsync(string newName)
        {
            var newFile = new SmbFile(_file.GetParent());
            await _file.RenameToAsync(newFile);

            if (_file.Exists())
                throw new Exception("Файл не переименован.");

            if (!newFile.Exists())
                throw new Exception("Файл не переименован.");

            _file = newFile;
        }
    }
}
