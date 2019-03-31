using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CFT.FileProvider.SMB
{
    public class SMBFileProviderOptions
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string ServerIP { get; set; }


        internal string FullPath
        {
            get
            {
                if (string.IsNullOrEmpty(Login))
                    return $"smb://{ServerIP }";

                return $"smb://{Login}:{Password}@{ServerIP }";
            }
        }

        public SMBFileProviderOptions() { }

        public void ValidationParams()
        {
            if (string.IsNullOrWhiteSpace(ServerIP))
                throw new ArgumentException("Не указан IP сервера.");

            if (!IPAddress.TryParse(ServerIP, out var ip))
                throw new ArgumentException($"Неверный формат IP '{ServerIP}'.");


            if (!string.IsNullOrWhiteSpace(Login) && string.IsNullOrWhiteSpace(Password))
                throw new ArgumentException("Не указан пароль.");
        }
    }
}
