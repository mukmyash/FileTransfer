using BenchmarkDotNet.Running;
using System;

namespace CFT.FileProvider.SMB.Benchmarks
{
    class Program
    {
        static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}
