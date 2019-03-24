using System;
using System.IO;
using System.Threading.Tasks;

namespace cft.Service.FTPWorker
{
    public interface IFTPScaner
    {
        Task Scan(Action<FileInfo> fileReceiveAction);
    }
}