using CFT.MiddleWare.Transformations.FileName.ParametersExtracter;
using CFT.MiddleWare.Transformations.FileName.ParametersExtracter.Options;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Transformations.FileName.Benchmarks.Mocks
{
    internal class ParameterExtracterMockFactory : IParameterExtracterFactory
    {
        private readonly Dictionary<string, string> result;

        public ParameterExtracterMockFactory(Dictionary<string, string> result)
        {
            this.result = result;
        }

        public ParameterExtracterBase GetParameterExtracterFlow(IConfigurationSection configSection)
        {
            return new ParameterExtracterBaseMock(result);
        }

        public ParameterExtracterBase GetParameterExtracterFlow(IEnumerable<ParameterDescriptionOptionBase> optionsExtracter)
        {
            return new ParameterExtracterBaseMock(result);
        }
    }
}
