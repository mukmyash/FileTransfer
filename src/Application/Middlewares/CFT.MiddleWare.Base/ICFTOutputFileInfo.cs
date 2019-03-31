using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Base
{
    public interface ICFTOutputFileInfo : ICFTInputFileInfo
    {
        byte[] FileContent { get; set; }
        string FileName { get; set; }
        string FullName { get; set; }
    }
}
