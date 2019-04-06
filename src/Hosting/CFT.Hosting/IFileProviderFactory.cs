using CFT.FileProvider.Abstractions;
using Microsoft.Extensions.Configuration;

namespace CFT.Hosting
{
    internal interface IFileProviderFactory
    {
        ICFTFileProvider GetFileProvider(string type, IConfigurationSection settings);
    }
}