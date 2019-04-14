using CFT.Application.Abstractions.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.Hosting
{
    public class FileScanerOptions
    {
        /// <summary>
        /// Кол-во паралельных обрабатываемых файлов.
        /// </summary>
        public int NumberParallelFileWork { get; set; } = 5;

        /// <summary>
        /// Каталог для быкапа файла. Если не указан бэкап не создается.
        /// </summary>
        public string BackupPath { get; set; }

        /// <summary>
        /// Создавать-ли бэкап.
        /// </summary>
        public bool UseBackup => !string.IsNullOrWhiteSpace(BackupPath);

        /// <summary>
        /// Путь который будет прослушиваться.
        /// </summary>
        public string WatchPath { get; set; }

        /// <summary>
        /// Период сканирования каталога. (в секундах)
        /// </summary>
        public int ScanPeriodSeconds { get; set; }

        /// <summary>
        /// Тип файлового провайдера.
        /// </summary>
        public string FileProviderType { get; set; }

        /// <summary>
        /// Конфигурация для файлового провайдера. (У каждого файлового провайдера своя конфигурация.)
        /// </summary>
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
