using CFT.MiddleWare.Base;
using Microsoft.Extensions.Configuration;
using MiddleWare.Abstractions;
using MiddleWare.Abstractions.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Manipulation.Export
{
    public static class MiddleWareBuilderExtension
    {
        public static IMiddlewareBuilder<CFTFileContext> UseExport(
            this IMiddlewareBuilder<CFTFileContext> app,
            IConfigurationSection configSection)
        {
            return app.UseExport(options =>
            {
                configSection.Bind(options);
            });
        }

        public static IMiddlewareBuilder<CFTFileContext> UseExport(
            this IMiddlewareBuilder<CFTFileContext> app,
            Action<ExportFileOptions> configOption)
        {
            var options = new ExportFileOptions();
            configOption(options);
            app.UseMiddleware<ExportFileMiddleWare, CFTFileContext>(options);
            return app;
        }
    }
}
