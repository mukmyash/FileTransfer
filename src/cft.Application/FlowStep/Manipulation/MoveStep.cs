using cft.Application.Exceptions;
using cft.Application.FileProvider;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cft.Application.FlowStep.Manipulation
{
    internal class MoveStep : IFlowStep
    {
        IFileProvider _fileProvider;
        string _path;

        public MoveStep(IMoveStepOptions options, IFileProviderFactory fileProviderFactory)
        {
            try
            {
                options.ValidationParams();
                _fileProvider = fileProviderFactory.GetProvider(options.FileProvider, options.Settings);
            }
            catch (Exception e)
            {
                throw new CFTConfigurationException("Ошибка конфигурации модуля переноса файла.", e);
            }

            _path = options.Path;
        }

        public async Task RunAsync(FileContext context)
        {
            await _fileProvider.CreateFileAsync(context.FileInfo, _path);
        }
    }
}
