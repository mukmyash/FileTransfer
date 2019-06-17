using CFT.Application.Abstractions.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace CFT.MiddleWare.Transformations.FileName
{
    public class FileNameTransformOptions
    {
        public string FileMask { get; set; }
        public IConfigurationSection ParametersDescription { get; set; }

        public void ValidationParams()
        {
            if (string.IsNullOrWhiteSpace(FileMask))
                throw new CFTConfigurationException("Не указана маска для преобразования.");

            if(ParametersDescription==null || !ParametersDescription.Exists())
                throw new CFTConfigurationException("Не указаны параметры для маски в файле.");
        }
    }
}
