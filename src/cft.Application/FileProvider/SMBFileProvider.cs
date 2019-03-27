using cft.Application.Exceptions;
using SharpCifs.Smb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace cft.Application.FileProvider
{
    internal class SMBFileProvider : IFileProvieder
    {
        string _login;
        string _password;
        string _serverIP;
        string _path;

        private string FullPath => $"smb://{_login}:{_password}@{_serverIP }/{_path }";

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
            _path = options.Path;
        }

        public Task CreateFileAsync(FileInfo fileInfo)
        {
            var file = new SmbFile($"{ FullPath }/{fileInfo.Name}.{fileInfo.Extension}");

            file.CreateNewFile();

            using (var writeStream = file.GetOutputStream())
            {
                writeStream.Write(File.ReadAllBytes(fileInfo.FullName));
            }

            return Task.CompletedTask;
        }
    }
}
