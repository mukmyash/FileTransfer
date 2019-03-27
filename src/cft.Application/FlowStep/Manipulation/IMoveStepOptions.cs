using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace cft.Application.FlowStep.Manipulation
{
    public class IMoveStepOptions
    {
        public string FileProvider { get; set; }
        public IConfigurationSection Settings { get; set; }
    }
}
