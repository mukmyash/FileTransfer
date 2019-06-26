using CFT.MiddleWare.Transformations.FileName.ParametersExtracter.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Transformations.FileName.ParametersExtracter
{
    internal class CurrentDateTimeExtracter : ParameterExtracterBase
    {
        public CurrentDateTimeExtracter(
            CurrentDateTimeDescriptionOptions options,
            ParameterExtracterBase next)
            : base(next)
        {
            _options = options;
        }

        CurrentDateTimeDescriptionOptions _options;

        public override Dictionary<string, string> Extract(ParameterContext ctx)
        {
            var result = _next?.Extract(ctx);
            if (result == null)
                result = new Dictionary<string, string>();

            result.Add(_options.ParameterName, DateTime.Now.ToString(_options.Format));
            return result;
        }
    }
}
