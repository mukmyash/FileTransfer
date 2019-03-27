using cft.Application.Options;

namespace cft.Application.FileProvider
{
    public interface ISMBFileProviderOptions: IValidateOptions
    {
        string Login { get; set; }
        string Password { get; set; }
        string Path { get; set; }
        string ServerIP { get; set; }
    }
}