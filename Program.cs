using System;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Threading;
using Parcs;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace ParcsCpppp
{
    internal class Program : IModule
    {
        static void Main(string[] args)
        {
            var job = new Job();
            if (!File.Exists(Assembly.GetExecutingAssembly().Location))
            {
                Console.WriteLine("File doesn't exist");
                return;
            }
            job.AddFile(Assembly.GetExecutingAssembly().Location);
            (new Program()).Run(new ModuleInfo(job, null));
            Console.ReadKey();
        }

        public void Run(ModuleInfo info, CancellationToken token = default)
        {
            var sw = new Stopwatch();
            var readingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DataFiles";

            List<int> items = new List<int>();
            int capacity = 0;

            ReaderWriter.read(readingPath + @"\input.txt", ref items, ref capacity);
            Console.WriteLine(capacity.ToString());
            for (int i = 0; i < items.Count(); i++)
            {
                Console.Write(items[i].ToString());
            }
            Console.WriteLine();

            sw.Start();
            const int pointsNum = 2;
            var points = new IPoint[pointsNum];
            var channels = new IChannel[pointsNum];
            for (var i = 0; i < pointsNum; i++)
            {
                points[i] = info.CreatePoint();
                channels[i] = points[i].CreateChannel();
                points[i].ExecuteClass("ParcsCpppp.Executor");
            }

            int step = Executor.factorial(items.Count()) / pointsNum;

            for (var i = 0; i < pointsNum; i++)
            {
                channels[i].WriteObject(items);
                channels[i].WriteObject(capacity);
                channels[i].WriteObject(i * step);
                channels[i].WriteObject((i+1) * step);
            }

            int minBinCount = items.Count();
            var totalResult = new List<List<Bin>>();
            for (var i = 0; i < pointsNum; i++)
            {
                var partialResult = channels[i].ReadObject<List<List<Bin>>>();

                for(int j = 0; j < partialResult.Count(); j++)
                {
                    var pr = partialResult[j];
                    Executor.updateAnswer(ref minBinCount, ref totalResult, ref pr);
                }
            }
            sw.Stop();

            ReaderWriter.write(readingPath + @"\output.txt", ref totalResult);

            Console.WriteLine($"Total time {sw.ElapsedMilliseconds} ms");
        }
    }
}
