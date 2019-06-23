using CFT.MiddleWare.Transformations.FileName.ParametersExtracter;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Transformations.FileName.Benchmarks.Mocks
{
    internal class ParameterExtracterBaseMock : ParameterExtracterBase
    {
        Dictionary<string, string> result;

        public ParameterExtracterBaseMock(Dictionary<string, string> result)
            : base(null)
        {
            this.result = result;
        }

        public override Dictionary<string, string> Extract(ParameterContext ctx)
        {
            return result;
        }
    }
}
