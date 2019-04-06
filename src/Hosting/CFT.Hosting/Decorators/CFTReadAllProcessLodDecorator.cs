using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CFT.MiddleWare.Base;
using Microsoft.Extensions.Logging;
using MiddleWare.Abstractions;

namespace CFT.Hosting.Decorators
{
    internal class CFTReadAllProcessLodDecorator : ICFTReadAllProcess
    {
        ICFTReadAllProcess _component;
        ILogger _logger;

        public CFTReadAllProcessLodDecorator(ICFTReadAllProcess component, ILogger<ICFTReadAllProcess> logger)
        {
            _component = component ?? throw new ArgumentNullException(nameof(component));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ProcessAllAsync(MiddlewareDelegate<CFTFileContext> applicationFlow)
        {
            try
            {
                _logger.LogInformation("Начинаем обработку файлов в каталоге.");
                await _component.ProcessAllAsync(applicationFlow);
                _logger.LogInformation("Файлы обработаны.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка обработки файлов.");
                throw;
            }
        }
    }
}
