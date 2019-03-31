using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace cft.Application.FlowStep.Manipulation
{
    public interface IMoveStepOptions : IValidateOptions
    {
        string FileProvider { get; }
        string Path { get; }
        IConfigurationSection Settings { get; }
    }
}
