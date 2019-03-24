using System.Threading.Tasks;

namespace cft.Application
{
    public interface IFlowRuner
    {
        Task Run(FileContext context);
    }
}