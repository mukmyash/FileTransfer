using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MiddleWare.Abstractions.Tests.Model
{
    public class TestContext : ContextBase
    {
        public TestContext(IServiceProvider applicationServices, IServiceProvider contextServices)
        {
            ApplicationServices = applicationServices;
            ContextServices = contextServices;
        }

        public StringBuilder Message { get; } = new StringBuilder();
        public override IServiceProvider ApplicationServices { get; }

        public override IServiceProvider ContextServices { get; }

        public override void Abort()
        {
            throw new OperationCanceledException();
        }
    }
}
