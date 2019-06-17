using CFT.MiddleWare.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CFT.MiddleWare.Transformations.FileName.ParametersExtracter
{
    /// <summary>
    /// Контекст для формирования параметров.
    /// </summary>
    internal class ParameterContext
    {
        public ParameterContext(CFTFileContext context)
        {
            AppContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public CFTFileContext AppContext { get; }

        public XmlElement XmlRootInput { get; set; }
        public XmlElement XmlRootOutput { get; set; }

        public static explicit operator ParameterContext(CFTFileContext appCtx)
        {
            return new ParameterContext(appCtx);
        }
    }
}
