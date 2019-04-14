using CFT.FileProvider.Abstractions;
using Microsoft.Extensions.Configuration;

namespace CFT.FileProvider
{
    public interface IFileProviderFactory
    {
        ICFTFileProvider GetFileProvider(string type, IConfigurationSection settings);
    }
}