using CFT.MiddleWare.Transformations.FileName.ParametersExtracter;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Transformations.FileName.Test.Mocks
{
    internal class MockParameterExtracterBase : ParameterExtracterBase
    {
        public MockParameterExtracterBase(ParameterExtracterBase next) : base(next)
        {
        }

        public override Dictionary<string, string> Extract(ParameterContext ctx)
        {
            return new Dictionary<string, string>()
                {
                    { "FP1","1"},
                    { "FP2","2"},
                    { "FP3","3"},
                };
        }
    }
}
