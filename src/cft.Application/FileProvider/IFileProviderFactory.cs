using Microsoft.Extensions.Configuration;

namespace cft.Application.FileProvider
{
    internal interface IFileProviderFactory
    {
        IFileProvider GetProvider(string type, IConfigurationSection config);
    }
}