using System.Collections.Generic;
using CFT.MiddleWare.Transformations.FileName.ParametersExtracter.Options;
using Microsoft.Extensions.Configuration;

namespace CFT.MiddleWare.Transformations.FileName.ParametersExtracter
{
    internal interface IParameterExtracterFactory
    {
        ParameterExtracterBase GetParameterExtracterFlow(IConfigurationSection configSection);
        ParameterExtracterBase GetParameterExtracterFlow(IEnumerable<ParameterDescriptionOptionBase> optionsExtracter);
    }
}