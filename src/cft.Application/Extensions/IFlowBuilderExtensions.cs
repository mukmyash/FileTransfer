using cft.Application.Exceptions;
using cft.Application.FlowStep.Transformation;
using cft.Application.FlowStep.Validation;
using cft.Application.Options;
using cft.Application.Options.FlowStep.Transformation;
using cft.Application.Options.FlowStep.Validation;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace cft.Application.Extensions
{
    public static class IFlowBuilderExtensions
    {
        public static IFlowBuilder RegisterTransformXSLT(this IFlowBuilder builder, IConfigurationSection section)
        {
            var options = section.Get<TransformXSLContentStepOptions>();
            var step = new TransformXSLContentStep(options);
            builder.RegisterStep(step);
            return builder;
        }

        public static IFlowBuilder RegisterTransformFileName(this IFlowBuilder builder, IConfigurationSection section)
        {
            var options = section.Get<TransformFileNameStepOptions>();
            var step = new TransformFileNameStep(options);
            builder.RegisterStep(step);
            return builder;
        }

        public static IFlowBuilder RegisterValidationDublicateByFileSystem(this IFlowBuilder builder, IConfigurationSection section)
        {
            var options = section.Get<CheckDublicateByFileSystemStepOptions>();
            var step = new CheckDublicateByFileSystemStep(options);
            builder.RegisterStep(step);
            return builder;
        }

        public static IFlowBuilder RegisterFromConfig(this IFlowBuilder builder, IConfigurationSection section)
        {
            var options = section.Get<StepInfoOptions[]>();

            foreach (var option in options)
            {
                switch (option.Type)
                {
                    case "CheckDublicate":
                        builder.RegisterValidationDublicateByFileSystem(option.Settings);
                        break;
                    case "TransforFileName":
                        builder.RegisterTransformFileName(option.Settings);
                        break;
                    case "TransforXSLContent":
                        builder.RegisterTransformXSLT(option.Settings);
                        break;
                    default:
                        throw new CFTApplicationException($"Шаг '{option.Type}' не определен.");
                }
            }

            return builder;
        }
    }
}
