using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace cft.Application.FileProvider
{
    internal interface IFileProvider
    {
        Task CreateFileAsync(FileInfo file, string path);
    }
}
