using System.Threading.Tasks;
using cft.Application.FlowStep;

namespace cft.Application
{
    public interface IFlowBuilder
    {
        void RegisterStep(IFlowStep step);
        IFlowRuner Build();
    }
}