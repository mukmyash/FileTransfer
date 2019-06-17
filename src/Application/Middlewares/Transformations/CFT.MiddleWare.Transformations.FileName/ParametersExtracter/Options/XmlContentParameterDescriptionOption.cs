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
            XPath = configSection.GetValue<string>("XPath")
                ?? throw new CFTConfigurationException($"Не указано XPath до параетра. ({nameof(XPath)})");
        }

        internal override ParameterType ParameterType => ParameterType.XMLContent;

        public string XPath { get; set; }

    }
}
