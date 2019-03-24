using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace cft.Service.Options
{
    public class FTPOptions
    {
        public string HOST { get;  set; }
        public int? Port { get;  set; }
        public int ScanPerion { get;  set; }
        public string PathScaning { get;  set; }
        public string DownloadFolder { get;  set; }
        public IConfigurationSection Steps { get;  set; }
    }
}
