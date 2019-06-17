using CFT.MiddleWare.Transformations.FileName.ParametersExtracter.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFT.MiddleWare.Transformations.FileName.ParametersExtracter
{
    internal class FileNameParameterExtracter : ParameterExtracterBase
    {
        public FileNameParameterExtracter(
            FileNameParameterDescriptionOption options,
            ParameterExtracterBase next)
            : base(next)
        {
            _options = options;
        }

        FileNameParameterDescriptionOption _options;

        public override Dictionary<string, string> Extract(ParameterContext ctx)
        {
            var result = _next?.Extract(ctx);
            if (result == null)
                result = new Dictionary<string, string>();

            int paramNumber = 0;

            string fileName;

            switch (_options.FileType)
            {
                case ExtractFileType.Input:
                    fileName = ctx.AppContext.InputFile.FileName;
                    break;
                case ExtractFileType.Output:
                    fileName = ctx.AppContext.OutputFile.FileName;
                    break;
                default:
                    throw new Exception("Данный тип файла не поддерживается.");
            }

            StringBuilder buffer = new StringBuilder();

            foreach (var c in fileName)
            {
                if (!_options.Separators.Contains(c))
                    buffer.Append(c);

                var parameterValue = buffer.ToString();
                parameterValue = string.IsNullOrEmpty(parameterValue) ? _options.DefaultValue : parameterValue;

                result.Add(string.Concat(_options.ParameterName, ++paramNumber), parameterValue);
                buffer.Clear();
            }
            var lastParameterValue = buffer.ToString();
            lastParameterValue = string.IsNullOrEmpty(lastParameterValue) ? _options.DefaultValue : lastParameterValue;

            result.Add(string.Concat(_options.ParameterName, ++paramNumber), lastParameterValue);

            return result;
        }
    }
}
