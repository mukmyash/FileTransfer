using CFT.Application.Abstractions.Exceptions;
using CFT.FileProvider.Abstractions;
using CFT.FileProvider.SMB;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.Hosting
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
                    throw new CFTConfigurationException($"Тип поставщика файлов '{type}' не поддерживается.");
            }
        }
    }
}
