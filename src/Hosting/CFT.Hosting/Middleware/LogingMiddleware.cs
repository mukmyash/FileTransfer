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
        readonly MiddlewareDelegate<CFTFileContext> _next;
        readonly ILogger _logger;
        readonly string _startMessage;
        readonly string _endSuccessMessage;
        readonly string _endErrorMessage;

        public LogMiddleware(
            MiddlewareDelegate<CFTFileContext> next,
            ILogger<LogMiddleware> logger,
            string startMessage,
            string endSuccessMessage,
            string endErrorMessage)
        {
            if (string.IsNullOrEmpty(startMessage))
                throw new ArgumentNullException(nameof(startMessage));
            if (string.IsNullOrEmpty(endSuccessMessage))
                throw new ArgumentNullException(nameof(endSuccessMessage));
            if (string.IsNullOrEmpty(endErrorMessage))
                throw new ArgumentNullException(nameof(endErrorMessage));

            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _startMessage = startMessage;
            _endSuccessMessage = endSuccessMessage;
            _endErrorMessage = endErrorMessage;
        }

        public async Task InvokeAsync(CFTFileContext context)
        {
            try
            {
                _logger.LogInformation(_startMessage);
                await _next.Invoke(context);
                _logger.LogInformation(_endSuccessMessage);
            }
            catch (Exception e)
            {
                _logger.LogError(e, _endErrorMessage);
            }
        }
    }

}
