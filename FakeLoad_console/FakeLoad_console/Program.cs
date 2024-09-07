using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace FakeLoad_console
    {
    internal class Program
        {
        static float cpu_limit = 10;
        static int load_threads = 2;

        static void CpuLoad()
            {
            for (long i = 0; i < 1000000000; i++)
                { Math.Sqrt(i); }
            Console.WriteLine(".");
        }
        static float GetCpuUsage()
            {
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();
            Thread.Sleep(1000);

            return cpuCounter.NextValue();
            }
        static void Main(string[] args)
            {
            Console.WriteLine("Starting..");
            while (true)
                {
                float pom = GetCpuUsage();
                if (pom > cpu_limit)
                    {
                    Console.WriteLine("System has enough load: " + pom);
                    Thread.Sleep(10000);
                    }
                else
                    {
                    Console.WriteLine("Runnign artificial load..");

                    Thread[] threads = new Thread[load_threads];
                    for (int i = 0; i < load_threads; i++)
                        {
                        threads[i] = new Thread(() => CpuLoad());
                        threads[i].IsBackground = true;
                        threads[i].Start();
                        }

                    foreach (Thread thread in threads)
                        { thread.Join(); }
                    }
                }
            }
        }
    }
