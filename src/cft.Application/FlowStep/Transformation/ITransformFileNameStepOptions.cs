﻿using cft.Application.Exceptions;
using cft.Application.Options;
using cft.Application.Options.FlowStep.Transformation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cft.Application.FlowStep.Transformation
{
    public interface ITransformFileNameStepOptions : IValidateOptions
    {
        string FileMask { get; }
    }
}
