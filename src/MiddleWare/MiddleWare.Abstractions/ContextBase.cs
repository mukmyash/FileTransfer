using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MiddleWare.Abstractions
{
    public abstract class ContextBase
    {
        public abstract IServiceProvider ContextServices { get; }
    }
}
