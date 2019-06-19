using CFT.Application.Abstractions.Exceptions;
using CFT.MiddleWare.Transformations.FileName.ParametersExtracter.Options;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Transformations.FileName.ParametersExtracter.Options
{
    internal class XmlContentParameterDescriptionOption : ParameterDescriptionOptionBase
    {
        public XmlContentParameterDescriptionOption(IConfigurationSection configSection)
            : base(configSection)
        {
            Init(configSection.GetValue<string>("XPath"));
        }

        public XmlContentParameterDescriptionOption(
            string xPath,
            string parameterName,
            ExtractFileType fileType,
            string defaultValue)
            : base(parameterName, fileType, defaultValue)
        {
            Init(xPath);
        }

        private void Init(string xPath)
        {
            XPath = !string.IsNullOrWhiteSpace(xPath) ?
                xPath :
                throw new CFTConfigurationException($"Не указано XPath до параетра. ({nameof(XPath)})");
        }

        internal override ParameterType ParameterType => ParameterType.XMLContent;

        public string XPath { get; set; }

    }
}
