using CFT.MiddleWare.Transformations.FileName.ParametersExtracter.Options;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Transformations.FileName.ParametersExtracter
{
    internal class ParameterExtracterFactory : IParameterExtracterFactory
    {
        private IParameterDescriptionOptionFactory _parameterDescriptionOptionFactory;

        public ParameterExtracterFactory(IParameterDescriptionOptionFactory parameterDescriptionOptionFactory)
        {
            _parameterDescriptionOptionFactory = parameterDescriptionOptionFactory;
        }

        public ParameterExtracterBase GetParameterExtracterFlow(
            IConfigurationSection configSection)
        {
            return GetParameterExtracterFlow(_parameterDescriptionOptionFactory.ParseParameters(configSection));
        }

        public ParameterExtracterBase GetParameterExtracterFlow(
            IEnumerable<ParameterDescriptionOptionBase> optionsExtracter)
        {
            ParameterExtracterBase result = new EmptyParameterExtracter();
            bool includeXmlPrepareExtracter = false;
            foreach (var option in optionsExtracter)
            {
                switch (option.ParameterType)
                {
                    case ParameterType.FileName:
                        result = new FileNameParameterExtracter(
                            option as FileNameParameterDescriptionOption, result);
                        break;
                    case ParameterType.XMLContent:
                        if (!includeXmlPrepareExtracter)
                        {
                            result = new XmlContentPrepareExtracter(result);
                            includeXmlPrepareExtracter = true;
                        }
                        result = new XmlContentParameterExtracter(
                            option as XmlContentParameterDescriptionOption, result);
                        break;
                    default:
                        throw new Exception("Данный тип параметра не поддерживается.");
                }
            }

            return result;
        }
    }
}
