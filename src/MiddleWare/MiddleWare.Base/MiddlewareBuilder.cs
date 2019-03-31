using MiddleWare.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleWare.Base
{
    public abstract class MiddlewareBuilder<TContext> : IMiddlewareBuilder<TContext>
        where TContext : ContextBase
    {
        private readonly IList<Func<MiddlewareDelegate<TContext>, MiddlewareDelegate<TContext>>> _components
            = new List<Func<MiddlewareDelegate<TContext>, MiddlewareDelegate<TContext>>>();

        public MiddlewareBuilder(IServiceProvider serviceProvider)
        {
            Properties = new Dictionary<string, object>();
            ApplicationServices = serviceProvider;
        }

        protected MiddlewareBuilder(MiddlewareBuilder<TContext> builder)
        {
            Properties = builder.Properties;
        }

        public IServiceProvider ApplicationServices
        {
            get
            {
                return GetProperty<IServiceProvider>("ApplicationServices");
            }
            set
            {
                SetProperty<IServiceProvider>("ApplicationServices", value);
            }
        }

        public IDictionary<string, object> Properties { get; }

        protected T GetProperty<T>(string key)
        {
            object value;
            return Properties.TryGetValue(key, out value) ? (T)value : default(T);
        }

        protected void SetProperty<T>(string key, T value)
        {
            Properties[key] = value;
        }

        public IMiddlewareBuilder<TContext> Use(Func<MiddlewareDelegate<TContext>, MiddlewareDelegate<TContext>> middleware)
        {
            _components.Add(middleware);
            return this;
        }

        public abstract IMiddlewareBuilder<TContext> New();
        //{
        //    return new MiddlewareBuilder<TContext>(this);
        //}

        public MiddlewareDelegate<TContext> Build(MiddlewareDelegate<TContext> defaultDelegate)
        {
            MiddlewareDelegate<TContext> app = defaultDelegate ?? (context =>
            {
                context.Abort();
                return Task.FromResult(0);
            });

            foreach (var component in _components.Reverse())
            {
                app = component(app);
            }

            return app;
        }

        public MiddlewareDelegate<TContext> Build()
        {
            return Build(null);
        }
    }
}
