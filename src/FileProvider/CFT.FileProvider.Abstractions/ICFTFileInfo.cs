using System;
using System.IO;

namespace CFT.FileProvider.Abstractions
{
    public interface ICFTFileInfo
    {
        bool Exists { get; }
        long Length { get; }
        string PhysicalPath { get; }
        string Name { get; }
        DateTimeOffset LastModified { get; }
        bool IsDirectory { get; }
        Stream CreateReadStream();
        void Rename(string v);
    }
}
