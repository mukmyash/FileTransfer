using CFT.MiddleWare.Base;
using Microsoft.Extensions.Configuration;
using MiddleWare.Abstractions;
using MiddleWare.Abstractions.Extensions;
using System;

namespace CFT.MiddleWare.Manipulation.Remove
{
    public static class MiddleWareBuilderExtension
    {
        public static IMiddlewareBuilder<CFTFileContext> UseRemove(
            this IMiddlewareBuilder<CFTFileContext> app,
            IConfigurationSection configSection)
        {
            return app.UseRemove(options =>
            {
                configSection.Bind(options);
            });
        }

        public static IMiddlewareBuilder<CFTFileContext> UseRemove(
            this IMiddlewareBuilder<CFTFileContext> app,
            Action<RemoveFileOptions> configOption)
        {
            var options = new RemoveFileOptions();
            configOption(options);
            app.UseMiddleware<RemoveFileMiddleWare, CFTFileContext>(options);
            return app;
        }
    }
}
