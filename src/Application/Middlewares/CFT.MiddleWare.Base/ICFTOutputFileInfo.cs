using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Base
{
    public interface ICFTOutputFileInfo : ICFTInputFileInfo
    {
        new byte[] FileContent { get; set; }
        new string FileName { get; set; }
        new string FullName { get; set; }
    }
}
