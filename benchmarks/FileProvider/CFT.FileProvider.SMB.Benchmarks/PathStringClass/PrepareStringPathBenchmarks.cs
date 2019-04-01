using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.FileProvider.SMB.Benchmarks.PathStringClass
{
    // [SimpleJob(RunStrategy.Monitoring, targetCount: 5)]
    [MemoryDiagnoser]
    [MinColumn, MaxColumn, MedianColumn]
    public class PrepareStringPathBenchmarks
    {
        [Params("smb://192.168.1/path/to/folder/"
            , "smb://192.168.2/path/to/folder/"
            , "path/to/folder/path/to/folder/"
            , "/path/to/folder/path/to/folder/"
            , "smb://192.168.1/path/to/folder"
            , "smb://192.168.2/path/to/folder"
            , "path/to/folder/path/to/folder"
            , "/path/to/folder/path/to/folder")]
        public string Path { get; set; }

        [Benchmark]
        public void PrepareStringPath(string path)
        {
            SMB.PathString.PrepareStringPath(Path, "smb://192.168.1/", true);
        }
    }
}
