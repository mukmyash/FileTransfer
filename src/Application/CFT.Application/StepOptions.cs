using Microsoft.Extensions.Configuration;
using System;

namespace CFT.Application
{
    public class StepOptions
    {
        public StepType Type { get; set; }
        public IConfigurationSection Settings {get;set;}
    }
}
