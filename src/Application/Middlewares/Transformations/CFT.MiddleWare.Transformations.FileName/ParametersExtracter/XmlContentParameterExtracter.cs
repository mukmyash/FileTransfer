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
            var result = _next?.Extract(ctx);
            if (result == null)
                result = new Dictionary<string, string>();

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

            var value =
                selectedNode == null ?
                null : selectedNode.NodeType != XmlNodeType.Attribute ?
                selectedNode.InnerText : selectedNode.Value;

            value = string.IsNullOrEmpty(value) ? _options.DefaultValue : value;

            result.Add(_options.ParameterName, value);
            return result;
        }
    }
}
