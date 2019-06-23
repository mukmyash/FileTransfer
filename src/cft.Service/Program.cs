using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using CFT.Hosting;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.ServiceProcess;

namespace cft.Service
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var startup = new Startup();

            var host = new CFTHostBuilder()
                .ConfigureServices(null)
                .ConfigureLogging(startup.ConfigureLoging)
                .ConfigureAppConfiguration(startup.Configure)
                .Build();


            var isService = !(Debugger.IsAttached || args.Contains("--console"));

            if (isService)
                host.RunAsWindowsService();
            else
                await host.RunAsync();
        }
    }

    public static class Extensions
    {
        public static void RunAsWindowsService(this IHost host)
        {
            var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);
            Directory.SetCurrentDirectory(pathToContentRoot);
            ServiceBase.Run(new CFTServiceBase(host));
        }
    }
}