using CFT.Hosting.Middleware;
using CFT.MiddleWare.Base;
using Microsoft.Extensions.Configuration;
using MiddleWare.Abstractions.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.Hosting.Extensions
{
    internal static class ICFTMiddlewareBuilderExtensions
    {
        public static ICFTMiddlewareBuilder UseBackup(this ICFTMiddlewareBuilder app, string path)
        {
            app.UseMiddleware<BackupInputFileMiddleware, CFTFileContext>(path);
            return app;
        }
    }
}
