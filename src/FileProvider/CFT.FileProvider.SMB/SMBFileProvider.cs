using CFT.FileProvider.Abstractions;
using Microsoft.Extensions.Primitives;
using SharpCifs.Smb;
using System;

namespace CFT.FileProvider.SMB
{
    public class SMBFileProvider : ICFTFileProvider
    {
        SMBFileProviderOptions _options;

        public SMBFileProvider(SMBFileProviderOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.ValidationParams();
            _options = options;
        }

        public ICFTDirectoryContents GetDirectoryContents(string subpath)
        {
            var smbPath = _options.FullPath;
            return new SMBDirectoryContents(new SmbFile($"{smbPath}/{PathString.PrepareStringPath(subpath, smbPath, true)}"));
        }

        public ICFTFileInfo GetFileInfo(string subpath)
        {
            var smbPath = _options.FullPath;
            return new SMBFileInfo(new SmbFile($"{smbPath}/{ PathString.PrepareStringPath(subpath, smbPath, false) }"));
        }

        public IChangeToken Watch(string subpath)
        {
            var smbPath = _options.FullPath;
            return new SMBDirectoryChangeToken($"{smbPath}/{PathString.PrepareStringPath(subpath, smbPath, true)}");
        }

        private string GetSubpath(string path)
        {
            if (path.StartsWith(_options.FullPath))
                return path.Substring(0, _options.FullPath.Length);

            if (path.StartsWith("smb://"))
            {
                var spanPath = path.AsSpan();
            }

            return path;
        }
    }
}
