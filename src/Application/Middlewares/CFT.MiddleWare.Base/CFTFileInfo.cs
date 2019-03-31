using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Base
{
    public class CFTFileInfo : ICFTOutputFileInfo
    {
        public CFTFileInfo(string fileName, byte[] fileContent, string fullName)
        {
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            FileContent = fileContent ?? throw new ArgumentNullException(nameof(fileContent));
            FullName = fullName;
        }

        internal CFTFileInfo(ICFTInputFileInfo fileInfo)
        {
            FileContent = new byte[fileInfo.FileContent.Length];
            Span<byte> fileContent = FileContent.AsSpan();
            FileContent.CopyTo(fileContent);
            FileName = fileInfo.FileName;
            FullName = string.Empty;
        }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public string FullName { get; set; }
    }
}
