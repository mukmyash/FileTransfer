using cft.Application.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace cft.Application.FlowStep.Validation
{
    public interface ICheckXSDStepOptions : IValidateOptions
    {
        string XSDPath { get; }
        string TargetNamespace { get; }
    }
}
