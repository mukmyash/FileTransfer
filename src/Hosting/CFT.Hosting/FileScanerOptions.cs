using CFT.Application.Abstractions.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.Hosting
{
    public class FileScanerOptions
    {
        public string BackupPath { get; set; }
        public bool UseBackup => !string.IsNullOrWhiteSpace(BackupPath);
        public string FileProviderType { get; set; }
        public string WatchPath { get; set; }
        public int ScanPeriodSeconds { get; set; }
        public IConfigurationSection FileProviderSettings { get; set; }

        public void ValidateOptions()
        {
            if (string.IsNullOrWhiteSpace(WatchPath))
                throw new CFTConfigurationException("Не указан путь сканирования.");

            if (string.IsNullOrWhiteSpace(FileProviderType))
                throw new CFTConfigurationException("Не указан тип FileProvider.");
        }
    }
}
