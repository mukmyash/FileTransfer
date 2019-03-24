using System;
using System.IO;

namespace cft.Application
{
    public class FileContext
    {
        public FileContext(FileInfo fileInfo)
        {
            FileInfo = fileInfo ?? throw new ArgumentNullException(nameof(fileInfo));
        }

        public FileInfo FileInfo { get; internal set; }
    }
}
