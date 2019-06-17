using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace CFT.MiddleWare.Transformations.FileName.ParametersExtracter.Options
{
    internal interface IParameterDescriptionOptionFactory
    {
        IEnumerable<ParameterDescriptionOptionBase> ParseParameters(IConfigurationSection options);
    }
}