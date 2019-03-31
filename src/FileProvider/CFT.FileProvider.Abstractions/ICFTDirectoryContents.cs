using System.Collections;
using System.Collections.Generic;

namespace CFT.FileProvider.Abstractions
{
    public interface ICFTDirectoryContents : IEnumerable<ICFTFileInfo>, IEnumerable
    {
        bool Exists { get; }
    }
}