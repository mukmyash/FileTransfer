using CFT.FileProvider.Abstractions;
using CFT.FileProvider.SMB;
using Microsoft.Extensions.Configuration;
using System;

namespace CFT.FileProvider
{
    public class FileProviderFactory : IFileProviderFactory
    {
        public ICFTFileProvider GetFileProvider(string type, IConfigurationSection settings)
        {
            switch (type.Trim().ToLower())
            {
                case "smb":
                    return new SMBFileProvider(settings.Get<SMBFileProviderOptions>());
                default:
                    throw new ApplicationException($"Тип поставщика файлов '{type}' не поддерживается.");
            }
        }
    }
}
