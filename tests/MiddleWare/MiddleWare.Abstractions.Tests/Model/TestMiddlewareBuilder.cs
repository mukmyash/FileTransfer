using System;
using System.Collections.Generic;
using System.Text;

namespace MiddleWare.Abstractions.Tests.Model
{
    public class TestMiddlewareBuilder : MiddlewareBuilderBase<TestContext>
    {
        public TestMiddlewareBuilder(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public TestMiddlewareBuilder(MiddlewareBuilderBase<TestContext> builder)
            : base(builder)
        {
        }

        public override IMiddlewareBuilder<TestContext> New()
        {
            return new TestMiddlewareBuilder(this);
        }
    }
}
