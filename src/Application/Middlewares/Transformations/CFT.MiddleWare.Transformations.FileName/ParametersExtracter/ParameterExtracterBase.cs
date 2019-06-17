using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Transformations.FileName.ParametersExtracter
{
    internal abstract class ParameterExtracterBase
    {
        protected ParameterExtracterBase _next;

        public ParameterExtracterBase(ParameterExtracterBase next)
        {
            _next = next;
        }

        public abstract Dictionary<string, string> Extract(ParameterContext ctx);
    }
}
