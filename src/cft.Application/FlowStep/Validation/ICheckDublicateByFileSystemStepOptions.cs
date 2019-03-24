using cft.Application.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace cft.Application.FlowStep.Validation
{
    public interface ICheckDublicateByFileSystemStepOptions : IValidateOptions
    {
        string PathStore { get; }
    }
}
