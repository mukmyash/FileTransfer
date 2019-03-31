using cft.Application.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace cft.Application.FileProvider
{
    public class SMBFileProviderOptions : ISMBFileProviderOptions
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string ServerIP { get; set; }

        public SMBFileProviderOptions() { }

        public SMBFileProviderOptions(IConfigurationSection config)
        {
            config.Bind(this);
        }

        public void ValidationParams()
        {
            if (string.IsNullOrWhiteSpace(ServerIP))
                throw new CFTConfigurationException("Не указан IP сервера.");

            if (!string.IsNullOrWhiteSpace(Login) && string.IsNullOrWhiteSpace(Password))
                throw new CFTConfigurationException("Не указан пароль.");
        }
    }
}
