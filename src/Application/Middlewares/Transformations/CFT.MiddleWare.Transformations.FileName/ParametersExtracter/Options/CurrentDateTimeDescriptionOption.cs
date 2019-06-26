using System;
using System.Collections.Generic;
using System.Text;
using CFT.Application.Abstractions.Exceptions;
using Microsoft.Extensions.Configuration;

namespace CFT.MiddleWare.Transformations.FileName.ParametersExtracter.Options
{
    internal class CurrentDateTimeDescriptionOptions : ParameterDescriptionOptionBase
    {
        public CurrentDateTimeDescriptionOptions(IConfigurationSection configSection) : base(configSection)
        {
            Init(configSection.GetValue<string>("Format"));
        }

        public CurrentDateTimeDescriptionOptions(string format, string parameterName, ExtractFileType fileType, string defaultValue) : base(parameterName, fileType, defaultValue)
        {
            Init(format);
        }

        private void Init(string format)
        {
            Format = !string.IsNullOrWhiteSpace(format) ?
                format :
                throw new CFTConfigurationException($"Не указано формат даты и времени. ({nameof(Format)})");

        }

        /// <summary>
        /// Формат даты и времени.
        /// <see cref="DateTime.ToString(string)"/>
        /// </summary>
        public string Format { get; set; }

        internal override ParameterType ParameterType => ParameterType.CurrentDateTime;
    }
}
