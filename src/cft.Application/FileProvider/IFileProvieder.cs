using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace cft.Application.FileProvider
{
    internal interface IFileProvieder
    {
        Task CreateFileAsync(FileInfo file);
    }
}
