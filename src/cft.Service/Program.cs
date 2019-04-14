using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using CFT.Hosting;

namespace cft.Service
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var startup = new Startup();

            await new CFTHostBuilder()
                .ConfigureServices(null)
                .ConfigureLogging(startup.ConfigureLoging)
                .ConfigureAppConfiguration(startup.Configure)
                .Build()
                .RunAsync();
        }
    }
}
