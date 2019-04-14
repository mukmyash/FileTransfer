using CFT.Application.Abstractions.Exceptions;
using Microsoft.Extensions.Configuration;
using System;

namespace CFT.MiddleWare.Manipulation.Remove
{
    public class RemoveFileOptions
    {
        public RemoveFileType FileType { get; set; }
        public string FileProviderType { get; set; }
        public IConfigurationSection FileProviderSettings { get; set; }

        internal void ValidationParams()
        {
            if (string.IsNullOrWhiteSpace(FileProviderType))
                throw new CFTConfigurationException("Не указан тип FileProvider.");
        }
    }
}
