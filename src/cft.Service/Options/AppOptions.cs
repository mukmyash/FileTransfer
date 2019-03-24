using System;
using System.Collections.Generic;
using System.Text;

namespace cft.Service.Options
{
    public class AppOptions
    {
        public string PathTmpFolder { get; }
        public int DelayFTPRequest_Secconds { get; }
        public FTPOptions FTP { get; }
    }
}
