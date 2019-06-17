using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace CFT.MiddleWare.Transformations.FileName.ParametersExtracter
{
    internal class XmlContentPrepareExtracter : ParameterExtracterBase
    {
        public XmlContentPrepareExtracter(ParameterExtracterBase next) : base(next)
        {
        }

        public override Dictionary<string, string> Extract(ParameterContext ctx)
        {
            ctx.XmlRootInput = CreateXmlElement(ctx.AppContext.InputFile.FileContent);
            ctx.XmlRootOutput = CreateXmlElement(ctx.AppContext.InputFile.FileContent);

            var result = _next?.Extract(ctx);
            if (result != null)
                return result;

            return new Dictionary<string, string>();
        }

        private XmlElement CreateXmlElement(byte[] fileContent)
        {
            var xmlDocument = new XmlDocument();
            using (var stream = new MemoryStream(fileContent))
            {
                xmlDocument.Load(stream);
            }

            return xmlDocument.DocumentElement;
        }
    }
}
