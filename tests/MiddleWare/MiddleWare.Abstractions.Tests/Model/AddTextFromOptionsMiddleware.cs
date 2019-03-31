using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiddleWare.Abstractions.Tests.Model
{
    public class AddTextFromOptionsMiddleware
    {
        MiddlewareDelegate<TestContext> _next;
        string _message;

        public AddTextFromOptionsMiddleware(MiddlewareDelegate<TestContext> next, string message)
        {
            _next = next;
            _message = message;
        }

        public Task InvokeAsync(TestContext context, AddTextFromOptionsMiddlewareOptions options)
        {

            context.Message.Append(_message);
            context.Message.AppendLine(options.Text);
            _next(context);
            return Task.CompletedTask;
        }
    }
}
