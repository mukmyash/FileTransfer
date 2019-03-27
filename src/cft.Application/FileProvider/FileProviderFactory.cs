using cft.Application.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace cft.Application.FileProvider
{
    internal class FileProviderFactory
    {
        public IFileProvieder GetProvider(string type, IConfigurationSection config)
        {
            switch (type.ToLower())
            {
                case "smb":
                    return new SMBFileProvider(new SMBFileProviderOptions(config));
                default:
                    throw new CFTConfigurationException($"Профайдер '{type}' для файлов не поддерживается.");
            }
        }
    }
}
