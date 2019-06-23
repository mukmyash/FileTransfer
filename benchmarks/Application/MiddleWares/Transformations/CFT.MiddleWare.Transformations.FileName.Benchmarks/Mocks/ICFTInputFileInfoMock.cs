using CFT.MiddleWare.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Transformations.FileName.Benchmarks.Mocks
{
    internal class ICFTInputFileInfoMock : ICFTInputFileInfo
    {
        public byte[] FileContent => Encoding.UTF8.GetBytes("Hello World");

        public string FileName => "FileName";

        public string FullName => "C:\\Test\\FileName";
    }
}
