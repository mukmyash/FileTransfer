using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Transformations.FileName.ParametersExtracter.Options
{
    internal class ParameterDescriptionOptionFactory : IParameterDescriptionOptionFactory
    {
        public IEnumerable<ParameterDescriptionOptionBase> ParseParameters(IConfigurationSection options)
        {
            int paramNumber = 0;

            while (true)
            {
                var section = options.GetSection((paramNumber++).ToString());
                if (!section.Exists())
                    break;

                switch (section.GetValue<ParameterType>("ParameterType"))
                {
                    case ParameterType.FileName:
                        yield return new FileNameParameterDescriptionOption(section);
                        break;
                    case ParameterType.XMLContent:
                        yield return new XmlContentParameterDescriptionOption(section);
                        break;
                    default:
                        throw new Exception("Данный тип параметра не поддерживается.");
                }
            }
        }
    }
}
