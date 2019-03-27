using cft.Service.Options;
using FluentFTP;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace cft.Service.FTPWorker
{
    public class FTPScaner : IFTPScaner
    {
        FTPOptions _options;
        IFtpClient _ftpClient;

        public FTPScaner(IOptionsSnapshot<FTPOptions> options, IFtpClient ftpClient)
        {
            _options = options.Value;
            _ftpClient = ftpClient;
        }

        public async Task Scan(Action<FileInfo> fileReceiveAction)
        {
            var client = _ftpClient;

            if (!client.IsConnected)
                client.Connect();

            foreach (FtpListItem item in client.GetListing(_options.PathScaning))
            {
                if (item.Type != FtpFileSystemObjectType.File)
                    continue;
                if (item.FullName.EndsWith("_CFT_WORK"))
                    continue;

                try
                {
                    // Помечаем файл взятым в работу (если вдруг работает много инстансов)
                    await client.RenameAsync(item.FullName, item.FullName + "_CFT_WORK");
                }
                catch (Exception)
                {
                    // Файл может быть взят другим в работу.
                    continue;
                }

                try
                {
                    var localFilePath = Path.Combine(_options.DownloadFolder, item.Name);
                    await client.DownloadFileAsync(localFilePath, item.FullName + "_CFT_WORK", FtpLocalExists.Skip, FtpVerify.None);

                    var file = new FileInfo(localFilePath);

                    fileReceiveAction?.Invoke(file);

                    await client.DeleteFileAsync(item.FullName + "_CFT_WORK");
                }
                catch (Exception)
                {
                    // Если что-то пошло не так то переименовываем обратно
                    if (await client.FileExistsAsync(item.FullName + "_CFT_WORK"))
                        await client.RenameAsync(item.FullName + "_CFT_WORK", item.FullName);
                }
            }
        }
    }
}
