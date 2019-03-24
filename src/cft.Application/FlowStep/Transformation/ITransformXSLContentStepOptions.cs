using cft.Application.Options;

namespace cft.Application.FlowStep.Transformation
{
    public interface ITransformXSLContentStepOptions : IValidateOptions
    {
        string XSLTPath { get; }
    }
}