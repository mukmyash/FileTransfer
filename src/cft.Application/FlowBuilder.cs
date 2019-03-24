using cft.Application.FlowStep;
using cft.Application.Options;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace cft.Application
{
    internal class FlowBuilder : IFlowBuilder
    {
        public Queue<IFlowStep> _steps = new Queue<IFlowStep>();

        public void RegisterStep(IFlowStep step)
        {
            _steps.Enqueue(step);
        }

        public IFlowRuner Build()
        {
            return new FlowRuner(_steps);
        }
    }
}
