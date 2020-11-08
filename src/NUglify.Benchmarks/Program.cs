using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnostics.Windows;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using NUglify.Html;
using BenchmarkDotNet.Attributes;
using ZetaProducerHtmlCompressor;

namespace NUglify.Benchmarks
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (true)
            {
                var config = ManualConfig.Create(DefaultConfig.Instance);
                var gcDiagnoser = new MemoryDiagnoser();
                config.Add(new Job
                {
                    Mode = Mode.SingleRun,
                    LaunchCount = 2,
                    WarmupCount = 2,
                    IterationTime = 1024,
                    TargetCount = 10
                });
                config.Add(gcDiagnoser);

                //var  config = DefaultConfig.Instance;
                //BenchmarkRunner.Run<BenchMinifier>(config);
                BenchmarkRunner.Run<BenchParser>(config);
            }
            else
            {
                var program = new BenchMinifier();

                var clock = Stopwatch.StartNew();

                GC.Collect(2, GCCollectionMode.Forced, true);

                var gc0 = GC.CollectionCount(0);
                var gc1 = GC.CollectionCount(1);
                var gc2 = GC.CollectionCount(2);

                program.BenchNUglify();

                clock.Stop();
                Console.WriteLine($" => time: {(double)clock.ElapsedMilliseconds / 1}ms (total {clock.ElapsedMilliseconds}ms)");
                DumpGC(gc0, gc1, gc2);

            }
        }

        static void DumpGC(int gc0, int gc1, int gc2)
        {
            Console.WriteLine($"gc0: {GC.CollectionCount(0) - gc0}");
            Console.WriteLine($"gc1: {GC.CollectionCount(1) - gc1}");
            Console.WriteLine($"gc2: {GC.CollectionCount(2) - gc2}");
        }
    }
}
