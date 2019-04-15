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
        /// <summary>
        /// Шаг проверки файла по XSD схеме.
        /// </summary>
        /// <param name="app">Сборщик мидлвар.</param>
        /// <param name="configSection">настройки для мидлвары</param>
        /// <returns>Сборщик мидлвар с добавленным шагом проверки по схеме.</returns>
        public static IMiddlewareBuilder<CFTFileContext> UseValidationXSD(
            this IMiddlewareBuilder<CFTFileContext> app,
            IConfigurationSection configSection)
        {
            return app.UseValidationXSD(options =>
            {
                configSection.Bind(options);
            });
        }

        /// <summary>
        /// Шаг проверки файла по XSD схеме.
        /// </summary>
        /// <param name="app">Сборщик мидлвар.</param>
        /// <param name="configOption">настройки для мидлвары</param>
        /// <returns>Сборщик мидлвар с добавленным шагом проверки по схеме.</returns>
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
