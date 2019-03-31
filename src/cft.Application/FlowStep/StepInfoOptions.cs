using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace cft.Application.FlowStep
{
    public class StepInfoOptions
    {
        public string Type { get; set; }
        public IConfigurationSection Settings { get; set; }
    }
}
