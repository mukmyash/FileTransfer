using CFT.Application.Abstractions.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Transformations.FileName.ParametersExtracter.Options
{
    internal abstract class ParameterDescriptionOptionBase
    {
        public ParameterDescriptionOptionBase(IConfigurationSection configSection)
        {
            ParameterName = configSection.GetValue<string>("ParameterName")
                ?? throw new CFTConfigurationException($"Не указано имя параметра. ({nameof(ParameterName)})");
            DefaultValue = configSection.GetValue<string>("DefaultValue");
            FileType = configSection.GetValue<ExtractFileType>("FileType");
        }

        public string ParameterName { get; }
        public ExtractFileType FileType { get; } = ExtractFileType.Input;
        public string DefaultValue { get; }
        internal abstract ParameterType ParameterType { get; }
    }
}
