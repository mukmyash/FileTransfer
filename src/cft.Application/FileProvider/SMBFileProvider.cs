using cft.Application.Exceptions;
using SharpCifs.Smb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cft.Application.FileProvider
{
    internal class SMBFileProvider : IFileProvider
    {
        string _login;
        string _password;
        string _serverIP;

        private string FullPath => $"smb://{_login}:{_password}@{_serverIP }";

        public SMBFileProvider(ISMBFileProviderOptions options)
        {
            try
            {
                options.ValidationParams();
            }
            catch (Exception e)
            {
                throw new CFTConfigurationException("Ошибка конфигурации SMB провайдера.", e);
            }

            _login = options.Login;
            _password = options.Password;
            _serverIP = options.ServerIP;
        }

        public Task CreateFileAsync(FileInfo fileInfo, string path)
        {
            var file = new SmbFile($"{ FullPath }/{path }/{fileInfo.Name}.{fileInfo.Extension}");

            file.CreateNewFile();

            using (var writeStream = file.GetOutputStream())
            {
                writeStream.Write(File.ReadAllBytes(fileInfo.FullName));
            }

            return Task.CompletedTask;
        }
    }
}
