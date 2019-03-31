using cft.Application.Exceptions;
using FluentFTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace cft.Application.FileProvider
{
    internal class FTPFileProvider : IFileProvider
    {
        IFtpClient _ftpClient;

        public async Task CreateFileAsync(FileInfo file, string path)
        {
            if (!_ftpClient.IsConnected)
                _ftpClient.Connect();

            string removefilePath = $"{path}/{file.Name}.{file.Extension}";

            if (await _ftpClient.FileExistsAsync(removefilePath))
                throw new CFTFileException($"Файл '{removefilePath}' уже существует в FTP репозитории. ({_ftpClient.Host})");

            await _ftpClient.UploadFileAsync(file.FullName, removefilePath);
        }
    }
}
