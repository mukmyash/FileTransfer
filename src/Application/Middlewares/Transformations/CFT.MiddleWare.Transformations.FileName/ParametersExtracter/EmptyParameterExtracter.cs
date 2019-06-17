using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Transformations.FileName.ParametersExtracter
{
    internal class EmptyParameterExtracter : ParameterExtracterBase
    {
        public EmptyParameterExtracter()
            : base(null)
        {

        }

        public override Dictionary<string, string> Extract(ParameterContext ctx)
        {
            return new Dictionary<string, string>();
        }
    }
}
