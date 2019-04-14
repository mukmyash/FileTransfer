using CFT.Application.Abstractions.Exceptions;
using Microsoft.Extensions.Configuration;
using System;

namespace CFT.MiddleWare.Manipulation.Export
{
    public class ExportFileOptions
    {
        public string Path { get; set; }
        public ExportFileType FileType { get; set; }
        public string FileProviderType { get; set; }
        public IConfigurationSection FileProviderSettings { get; set; }

        internal void ValidationParams()
        {
            if (string.IsNullOrWhiteSpace(Path))
                throw new CFTConfigurationException("Не указан тип путь для экспорта.");
            if (string.IsNullOrWhiteSpace(FileProviderType))
                throw new CFTConfigurationException("Не указан тип FileProvider.");
        }
    }
}
