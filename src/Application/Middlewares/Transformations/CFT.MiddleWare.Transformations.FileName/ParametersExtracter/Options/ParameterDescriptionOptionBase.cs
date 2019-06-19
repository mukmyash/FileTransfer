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
            : this(
                 parameterName: configSection.GetValue<string>("ParameterName"),
                 fileType: configSection.GetValue<ExtractFileType>("FileType"),
                 defaultValue: configSection.GetValue<string>("DefaultValue"))
        {
        }

        protected ParameterDescriptionOptionBase(string parameterName, ExtractFileType fileType, string defaultValue)
        {
            ParameterName = !string.IsNullOrWhiteSpace(parameterName) ? parameterName
                : throw new CFTConfigurationException($"Не указано имя параметра. ({nameof(ParameterName)})");
            FileType = fileType;
            DefaultValue = defaultValue;
        }

        public string ParameterName { get; }
        public ExtractFileType FileType { get; } = ExtractFileType.Input;
        public string DefaultValue { get; }
        internal abstract ParameterType ParameterType { get; }
    }
}
