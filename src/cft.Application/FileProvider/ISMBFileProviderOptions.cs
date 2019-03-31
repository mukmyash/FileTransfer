using cft.Application.FlowStep;

namespace cft.Application.FileProvider
{
    public interface ISMBFileProviderOptions: IValidateOptions
    {
        string Login { get; set; }
        string Password { get; set; }
        string ServerIP { get; set; }
    }
}