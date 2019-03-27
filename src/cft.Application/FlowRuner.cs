using cft.Application.Exceptions;
using cft.Application.FlowStep;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace cft.Application
{
    internal class FlowRuner : IFlowRuner
    {
        public Queue<IFlowStep> _steps = new Queue<IFlowStep>();

        public FlowRuner(Queue<IFlowStep> steps)
        {
            if ((steps?.Count ?? 0) == 0)
                throw new CFTApplicationException("Отсутствуют шаги для запуска");

            _steps = steps;
        }

        public async Task Run(FileContext context)
        {
            foreach (var stepFlow in _steps)
            {
                await stepFlow.RunAsync(context);
            }
        }
    }
}
