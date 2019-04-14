using CFT.MiddleWare.Base;
using Microsoft.Extensions.Configuration;
using MiddleWare.Abstractions;
using MiddleWare.Abstractions.Extensions;
using System;

namespace CFT.MiddleWare.Transformations.FileName
{
    public static class MiddleWareBuilderExtension
    {
        public static IMiddlewareBuilder<CFTFileContext> UseTransformFileName(
           this IMiddlewareBuilder<CFTFileContext> app,
           IConfigurationSection configSection)
        {
            return app.UseTransformFileName(options =>
            {
                configSection.Bind(options);
            });
        }

        public static IMiddlewareBuilder<CFTFileContext> UseTransformFileName(
            this IMiddlewareBuilder<CFTFileContext> app,
            Action<FileNameTransformOptions> configOption)
        {
            var options = new FileNameTransformOptions();
            configOption(options);
            app.UseMiddleware<FileNameTransformMiddleWare, CFTFileContext>(options);
            return app;
        }
    }
}
