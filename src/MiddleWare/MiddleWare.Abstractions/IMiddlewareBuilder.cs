using System;
using System.Collections.Generic;

namespace MiddleWare.Abstractions
{
    public interface IMiddlewareBuilder<TContext>
            where TContext : ContextBase
    {
        IServiceProvider ApplicationServices { get; set; }

        IDictionary<string, object> Properties { get; }

        IMiddlewareBuilder<TContext> Use(Func<MiddlewareDelegate<TContext>, MiddlewareDelegate<TContext>> middleware);

        IMiddlewareBuilder<TContext> New();

        MiddlewareDelegate<TContext> Build();
    }
}
