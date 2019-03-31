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
            return new SMBDirectoryContents(new SmbFile($"{_options.FullPath}/{ subpath }"));
        }

        public ICFTFileInfo GetFileInfo(string subpath)
        {
            return new SMBFileInfo(new SmbFile($"{_options.FullPath}/{ subpath }"));
        }

        public IChangeToken Watch(string subpath)
        {
            return new SMBDirectoryChangeToken($"{_options.FullPath}/{ subpath }");
        }
    }
}
