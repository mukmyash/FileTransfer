using System.Threading.Tasks;
using CFT.MiddleWare.Base;
using MiddleWare.Abstractions;

namespace CFT.Hosting
{
    internal interface ICFTReadAllProcess
    {
        Task ProcessAllAsync(MiddlewareDelegate<CFTFileContext> applicationFlow);
    }
}