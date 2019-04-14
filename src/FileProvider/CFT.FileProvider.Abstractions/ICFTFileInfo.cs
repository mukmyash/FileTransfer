using System;
using System.IO;
using System.Threading.Tasks;

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
        Stream CreateWriteStream();
        Task RenameAsync(string newName);
        void Delete();
    }
}
