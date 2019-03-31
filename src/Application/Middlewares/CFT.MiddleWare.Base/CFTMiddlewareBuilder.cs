using MiddleWare.Abstractions;
using System;

namespace CFT.MiddleWare.Base
{
    public class CFTMiddlewareBuilder : MiddlewareBuilderBase<CFTFileContext>, ICFTMiddlewareBuilder
    {
        public CFTMiddlewareBuilder(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public CFTMiddlewareBuilder(MiddlewareBuilderBase<CFTFileContext> builder)
            : base(builder)
        {
        }

        public override IMiddlewareBuilder<CFTFileContext> New()
        {
            return new CFTMiddlewareBuilder(this);
        }
    }
}
