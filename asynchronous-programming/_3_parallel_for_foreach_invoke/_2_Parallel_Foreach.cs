using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace _3_parallel_for_foreach_invoke
{
    public class _2_Parallel_Foreach
    {
        public void DoWork()
        {
            var numbers = Enumerable.Range(0, 10);

            //10 times, execute the SomeWork function.
            Parallel.ForEach(numbers, item =>
            {
                SomeWork();
            });
        }

        public void DoWork2()
        {
            ConcurrentBag<int> myNumbers = new ConcurrentBag<int>();

            var numbers = Enumerable.Range(0, 1000);

            var t = Task.Run(() =>
               {
                   Parallel.ForEach(numbers, item =>
                   {
                       myNumbers.Add(item);
                   });
               });
            t.Wait();
            Console.WriteLine("working with concurrent bag...");
            Console.WriteLine($"item count : {myNumbers.Count}");
        }

        private void SomeWork()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"{i} - {Thread.CurrentThread.ManagedThreadId}");
            }
        }
    }
}
