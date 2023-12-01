using System;
using System.Threading;
using System.Threading.Tasks;

namespace _3_parallel_for_foreach_invoke
{
    public class _3_Parallel_Invoke
    {
        //runs concurrently
        public void DoWork()
        {
            Parallel.Invoke(
                () => { Console.WriteLine("invoke 1 started.."); this.SomeWork("invoke 1", 1000); },
                () => { Console.WriteLine("invoke 2 started.."); this.SomeWork("invoke 2", 100); },
                () => { Console.WriteLine("invoke 3 started.."); this.SomeWork("invoke 3", 5000); }
                );
        }

        public void DoWork2()
        {
            ParallelOptions parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = 3
            };
            //Task.Run(() =>
            //{
            //normally parallel invoke blocks the mainthread until completed.
            //you can wrap with Task.Run to get better ui experiences
            Parallel.Invoke(
            parallelOptions,
            SomeWorkNoParameter,
            delegate () { Console.WriteLine("invoke 2 started.."); this.SomeWork("invoke 2", 100); },
            () => { Console.WriteLine("invoke 3 started.."); this.SomeWork("invoke 3", 5000); }
            );

            //});

            Console.WriteLine("end of work 2");
        }

        private void SomeWorkNoParameter()
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine($"{i} - {Thread.CurrentThread.ManagedThreadId}");
            }
        }

        private void SomeWork(string name, int limit)
        {
            for (int i = 0; i < limit; i++)
            {
                Console.WriteLine($"{i} - {Thread.CurrentThread.ManagedThreadId} from {name}");
            }
        }
    }
}
