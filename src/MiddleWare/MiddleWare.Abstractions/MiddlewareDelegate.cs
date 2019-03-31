using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiddleWare.Abstractions
{
    public delegate Task MiddlewareDelegate<TContext>(TContext context)
            where TContext : ContextBase;
}
