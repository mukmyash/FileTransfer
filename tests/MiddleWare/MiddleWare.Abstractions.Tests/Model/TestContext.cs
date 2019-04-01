using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MiddleWare.Abstractions.Tests.Model
{
    public class TestContext : ContextBase
    {
        public TestContext(IServiceProvider contextServices)
        {
            ContextServices = contextServices;
        }

        public StringBuilder Message { get; } = new StringBuilder();

        public override IServiceProvider ContextServices { get; }

    }
}
