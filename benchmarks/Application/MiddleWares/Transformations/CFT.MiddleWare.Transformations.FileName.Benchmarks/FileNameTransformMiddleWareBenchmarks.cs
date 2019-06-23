using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using CFT.MiddleWare.Transformations.FileName;
using System.Threading.Tasks;
using CFT.MiddleWare.Transformations.FileName.Benchmarks.Mocks;
using Microsoft.Extensions.DependencyInjection;
using CFT.MiddleWare.Base;

namespace CFT.MiddleWare.Transformations.FileName.Benchmarks
{
    [MemoryDiagnoser]
    [MinColumn, MaxColumn, MedianColumn]
    public class FileNameTransformMiddleWareBenchmarks
    {
        private readonly FileNameTransformMiddleWare testClass = new FileNameTransformMiddleWare(
                next: n => Task.CompletedTask,
                logger: new ILoggerMock(),
                parameterExtracterFactory: new ParameterExtracterMockFactory(result: new Dictionary<string, string>()
                {
                    {"FP1", "1" },
                    {"FP2", "Alfa" },
                    {"FP3", "Future" },
                    {"FP4", "People" }
                }),
                options: new FileNameTransformOptions()
                {
                    FileMask = "@{FP1}_@{FP2}-@{FP3}-{FP4}",
                    ParametersDescription = new IConfigurationSectionMock()
                }
            );

        readonly IServiceProvider serviceProvider = new ServiceCollection().BuildServiceProvider();
        readonly ICFTInputFileInfo ICFTInputFileInfo = new ICFTInputFileInfoMock();

        [Benchmark]
        public void PrepareStringPath()
        {
            testClass.InvokeAsync(
                new CFTFileContext(
                    serviceProvider, ICFTInputFileInfo
                    )
                    ).GetAwaiter().GetResult();
        }
    }
}
