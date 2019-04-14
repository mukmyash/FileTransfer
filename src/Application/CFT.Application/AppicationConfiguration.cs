using CFT.MiddleWare.Base;
using CFT.MiddleWare.Manipulation.Export;
using CFT.MiddleWare.Manipulation.Remove;
using CFT.MiddleWare.Transformations.FileName;
using CFT.MiddleWare.Transformations.XSLT;
using CFT.MiddleWare.Validation.XSD;
using Microsoft.Extensions.Configuration;
using MiddleWare.Abstractions;
using MiddleWare.Abstractions.Extensions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CFT.Application
{
    public class AppicationConfiguration
    {
        private string _scanFileProviderType;
        private readonly IConfigurationSection _scanFileProviderSettings;

        public AppicationConfiguration(
            string scanFileProviderType, 
            IConfigurationSection scanFileProviderSettings)
        {
            _scanFileProviderType = scanFileProviderType ?? throw new ArgumentNullException(nameof(scanFileProviderType));
            _scanFileProviderSettings = scanFileProviderSettings ?? throw new ArgumentNullException(nameof(scanFileProviderSettings));
        }

        public IMiddlewareBuilder<CFTFileContext> ConfigureApplication(
            IMiddlewareBuilder<CFTFileContext> middlewareBuilder,
            IConfigurationSection stepOptions)
        {
            return ConfigureApplication(
                middlewareBuilder,
                stepOptions.Get<IEnumerable<StepOptions>>());
        }

        public IMiddlewareBuilder<CFTFileContext> ConfigureApplication(
            IMiddlewareBuilder<CFTFileContext> middlewareBuilder,
            IEnumerable<StepOptions> stepsOptions)
        {
            foreach (var stepOption in stepsOptions)
            {
                switch (stepOption.Type)
                {
                    case StepType.MapWhenException:
                        MapWhenException(middlewareBuilder, stepOption.Settings);
                        break;
                    case StepType.MapWhenFileName:
                        MapWhenRegExpFileName(middlewareBuilder, stepOption.Settings);
                        break;
                    case StepType.XSDValidation:
                        middlewareBuilder.UseValidationXSD(stepOption.Settings);
                        break;
                    case StepType.FileNameTransformation:
                        middlewareBuilder.UseTransformFileName(stepOption.Settings);
                        break;
                    case StepType.XSLTransformation:
                        middlewareBuilder.UseTransformXSLT(stepOption.Settings);
                        break;
                    case StepType.Export:
                        middlewareBuilder.UseExport(stepOption.Settings);
                        break;
                }
            }

            middlewareBuilder.UseRemove(option =>
            {
                option.FileProviderType = _scanFileProviderType;
                option.FileProviderSettings = _scanFileProviderSettings;
                option.FileType = RemoveFileType.Input;
            });

            return middlewareBuilder;
        }

        #region MapWhenRegExpFileName
        public void MapWhenRegExpFileName(
            IMiddlewareBuilder<CFTFileContext> middlewareBuilder,
            IConfigurationSection mapWhenFileNameOptions)
        {
            MapWhenRegExpFileName(
                middlewareBuilder,
                mapWhenFileNameOptions.Get<MapWhenFileNameOptions>());
        }

        public void MapWhenRegExpFileName(
            IMiddlewareBuilder<CFTFileContext> middlewareBuilder,
            MapWhenFileNameOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options), "Отсутствуют настройки для секции определения имени файла.");
            options.ValidationParams();
            middlewareBuilder.MapWhen(
                ctx => Regex.Match(ctx.InputFile.FileName, options.Mask).Success,
                builder => this.ConfigureApplication(builder, options.Steps));
        }
        #endregion

        #region MapWhenException
        public void MapWhenException(
            IMiddlewareBuilder<CFTFileContext> middlewareBuilder,
            IConfigurationSection mapWhenExceptionOptions)
        {
            MapWhenException(
                middlewareBuilder,
                mapWhenExceptionOptions.Get<MapWhenExceptionOptions>());
        }

        public void MapWhenException(
            IMiddlewareBuilder<CFTFileContext> middlewareBuilder,
            MapWhenExceptionOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options), "Отсутствуют настройки для секции обработки ошибок.");
            options?.ValidationParams();
            middlewareBuilder.MapWhenException(
                builder => this.ConfigureApplication(builder, options.Steps));
        }
        #endregion
    }
}
