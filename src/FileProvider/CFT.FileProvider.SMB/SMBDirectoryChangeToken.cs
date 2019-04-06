using Microsoft.Extensions.Primitives;
using SharpCifs.Smb;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.FileProvider.SMB
{
    internal class SMBDirectoryChangeToken : IChangeToken
    {
        readonly string _folderPath;

        public SMBDirectoryChangeToken(string folderPath)
        {
            var directory = new SmbFile(folderPath);

            if (!directory.IsDirectory())
            {
                throw new ArgumentException($"'{directory.GetName()}' не является каталогом.");
            }
            if (!directory.Exists())
            {
                throw new ArgumentException($"Каталог '{directory.GetName()}' не существует.");
            }

            _folderPath = folderPath;
        }

        private long _lastModify;
        public bool HasChanged
        {
            get
            {
                var directory = new SmbFile(_folderPath);
                var currentModified = directory.LastModified();
                if (_lastModify != currentModified)
                {
                    _lastModify = currentModified;
                    return true;
                }

                return false;
            }
        }

        public bool ActiveChangeCallbacks => false;

        public IDisposable RegisterChangeCallback(Action<object> callback, object state)
        {
            return EmptyDisposable.Instance;
        }
    }
}
