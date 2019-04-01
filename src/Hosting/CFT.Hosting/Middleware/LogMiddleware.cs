using CFT.MiddleWare.Base;
using Microsoft.Extensions.Logging;
using MiddleWare.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CFT.Hosting.Middleware
{
    internal class LogMiddleware
    {
        MiddlewareDelegate<CFTFileContext> _next;
        ILogger _logger;

        public LogMiddleware(MiddlewareDelegate<CFTFileContext> next, ILogger<LogMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(CFTFileContext context)
        {
            using (_logger.BeginScope($"file_name = {context.InputFile.FileName}"))
            {
                using (_logger.BeginScope($"full_name = {context.InputFile.FullName}"))
                {
                    using (_logger.BeginScope($"corelation_id = {Guid.NewGuid()}"))
                    {
                        try
                        {
                            _logger.LogInformation("Начинаем обработку файла.");
                            await _next.Invoke(context);
                            _logger.LogInformation("Файл успешно обработан.");
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e, "Ошибка обработки файла.");
                        }
                    }
                }
            }
        }

        // Если будем использовать не ConsoleLogger то раскоментировать и результат функции передать в BeginScope
        //private Dictionary<string, object> GetScope(CFTFileContext context) => new Dictionary<string, object>
        //{
        //    ["file_name"] = context.InputFile.FileName,
        //    ["full_name"] = context.InputFile.FullName,
        //    ["corelation_id"] = Guid.NewGuid().ToString()
        //};
    }
}
