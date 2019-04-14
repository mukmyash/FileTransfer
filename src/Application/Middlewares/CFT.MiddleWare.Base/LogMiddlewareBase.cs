using Microsoft.Extensions.Logging;
using MiddleWare.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CFT.MiddleWare.Base
{
    public abstract class LogMiddlewareBase
    {
        readonly MiddlewareDelegate<CFTFileContext> _next;
        readonly ILogger _logger;
        protected abstract string StartMessage { get; }
        protected abstract string EndSuccessMessage { get; }
        protected abstract string EndErrorMessage { get; }

        public LogMiddlewareBase(
            MiddlewareDelegate<CFTFileContext> next,
            ILogger logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(CFTFileContext context)
        {
            try
            {
                _logger.LogInformation(StartMessage);
                await ExecAsync(context);
                _logger.LogInformation(EndSuccessMessage);
            }
            catch (Exception e)
            {
                _logger.LogError(e, EndErrorMessage);
                throw;
            }

            try
            {
                await _next.Invoke(context);
            }
            catch (Exception e)
            {
                await NextExceptionExecAsync(e, context);
                throw;
            }
        }

        protected abstract Task ExecAsync(CFTFileContext context);
        protected abstract Task NextExceptionExecAsync(Exception e, CFTFileContext context);
    }
}
