using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cft.Application.FlowStep
{
    public interface IFlowStep
    {
        Task Run(FileContext context);
    }
}
