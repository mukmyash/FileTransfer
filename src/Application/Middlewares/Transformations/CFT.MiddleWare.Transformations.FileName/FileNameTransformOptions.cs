using CFT.Application.Abstractions.Exceptions;
using System;

namespace CFT.MiddleWare.Transformations.FileName
{
    public class FileNameTransformOptions
    {
        public string FileMask { get; set; }

        public void ValidationParams()
        {
            if (string.IsNullOrWhiteSpace(FileMask))
                throw new CFTConfigurationException("Не указана маска для преобразования.");
        }
    }
}
