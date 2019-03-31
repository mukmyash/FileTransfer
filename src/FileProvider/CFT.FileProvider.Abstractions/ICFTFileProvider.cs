using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.FileProvider.Abstractions
{
    public interface ICFTFileProvider
    {
        ICFTDirectoryContents GetDirectoryContents(string subpath);
        ICFTFileInfo GetFileInfo(string subpath);
        IChangeToken Watch(string filter);
    }
}
