using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace cft.Service
{
    internal class CFTServiceBase : ServiceBase
    {
        private readonly IHost _host;

        public CFTServiceBase(IHost host)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
        }
    }
}
