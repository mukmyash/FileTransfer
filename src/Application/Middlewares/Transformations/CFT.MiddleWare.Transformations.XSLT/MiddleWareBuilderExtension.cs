using CFT.MiddleWare.Base;
using Microsoft.Extensions.Configuration;
using MiddleWare.Abstractions;
using MiddleWare.Abstractions.Extensions;
using System;

namespace CFT.MiddleWare.Transformations.XSLT
{
    public static class MiddleWareBuilderExtension
    {
        public static IMiddlewareBuilder<CFTFileContext> UseTransformXSLT(
           this IMiddlewareBuilder<CFTFileContext> app,
           IConfigurationSection configSection)
        {
            return app.UseTransformXSLT(options =>
            {
                configSection.Bind(options);
            });
        }

        public static IMiddlewareBuilder<CFTFileContext> UseTransformXSLT(
            this IMiddlewareBuilder<CFTFileContext> app,
            Action<XSLTransformContentOptions> configOption)
        {
            var options = new XSLTransformContentOptions();
            configOption(options);
            app.UseMiddleware<XSLTransformContentMiddleWare, CFTFileContext>(options);
            return app;
        }
    }
}
