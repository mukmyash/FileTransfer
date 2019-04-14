using CFT.MiddleWare.Base;
using Microsoft.Extensions.Configuration;
using MiddleWare.Abstractions;
using MiddleWare.Abstractions.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Validation.XSD
{
    public static class MiddleWareBuilderExtension
    {
        public static IMiddlewareBuilder<CFTFileContext> UseValidationXSD(
            this IMiddlewareBuilder<CFTFileContext> app,
            IConfigurationSection configSection)
        {
            return app.UseValidationXSD(options =>
            {
                configSection.Bind(options);
            });
        }

        public static IMiddlewareBuilder<CFTFileContext> UseValidationXSD(
            this IMiddlewareBuilder<CFTFileContext> app,
            Action<ValidateByXSDOptions> configOption)
        {
            var options = new ValidateByXSDOptions();
            configOption(options);
            app.UseMiddleware<ValidateByXSDMiddleWare, CFTFileContext>(options);
            return app;
        }
    }
}
