using CFT.MiddleWare.Transformations.FileName.ParametersExtracter.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CFT.MiddleWare.Transformations.FileName.ParametersExtracter
{
    internal class XmlContentParameterExtracter : ParameterExtracterBase
    {
        public XmlContentParameterExtracter(
            XmlContentParameterDescriptionOption options,
            ParameterExtracterBase next)
            : base(next)
        {
            _options = options;
        }

        XmlContentParameterDescriptionOption _options;

        public override Dictionary<string, string> Extract(ParameterContext ctx)
        {
            var result = _next.Extract(ctx);
            XmlNode selectedNode;
            switch (_options.FileType)
            {
                case ExtractFileType.Input:
                    selectedNode = ctx.XmlRootInput.SelectSingleNode(_options.XPath);
                    break;
                case ExtractFileType.Output:
                    selectedNode = ctx.XmlRootOutput.SelectSingleNode(_options.XPath);
                    break;
                default:
                    throw new Exception("Данный тип файла не поддерживается.");
            }

            var value = string.IsNullOrEmpty(selectedNode?.Value) ? _options.DefaultValue : selectedNode.Value;

            result.Add(_options.ParameterName, value);
            return result;
        }
    }
}
