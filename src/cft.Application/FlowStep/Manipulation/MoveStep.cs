using cft.Application.FileProvider;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cft.Application.FlowStep.Manipulation
{
    internal class MoveStep : IFlowStep
    {
        private IFileProvieder _fileProvider;

        public async Task RunAsync(FileContext context)
        {
            await _fileProvider.CreateFileAsync(context.FileInfo);
        }
    }
}
