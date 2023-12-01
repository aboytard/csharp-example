using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;

namespace _5_parallel_linq
{
    public class _1_Plinq
    {

        public void DoWork3()
        {
            var evenNumbers = from x in ((ParallelQuery<int>)ParallelEnumerable.Range(0, 50000))
                              where x % 2 == 0
                              select x;

            evenNumbers.ForAll(x => Console.WriteLine(x));
        }

        public void DoWork2()
        {
            ConcurrentBag<int> concurrentBag = new ConcurrentBag<int>();

            var result = Enumerable.Range(1, 100000);
            var oddNumbers = from x in result.AsParallel().AsOrdered()
                                 //.WithDegreeOfParallelism(1)
                             where x % 2 == 1
                             select x;

            var sw = Stopwatch.StartNew();
            try
            {
                oddNumbers.ForAll(Console.WriteLine);
                oddNumbers.ForAll(x => concurrentBag.Add(x));
                Console.WriteLine(concurrentBag.Count());
            }
            catch (AggregateException e)
            {
                foreach (var ex in e.InnerExceptions)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            sw.Stop();
            Console.WriteLine($"Total time: {sw.ElapsedMilliseconds} ms");
        }

        public void DoWork()
        {
            //there is no quarantee this will run in parallel
            var result = Enumerable.Range(1, 100000)
                .AsParallel()
                .WithDegreeOfParallelism(3)
                //.AsOrdered()
                .Select(Calculate)
                .Take(5);
            result.ForAll(Console.WriteLine);
        }

        private double Calculate(int number)
        {
            return Math.Pow(number, number);
        }
    }
}
