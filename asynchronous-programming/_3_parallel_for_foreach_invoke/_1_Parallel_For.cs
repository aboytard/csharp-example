using System;
using System.Threading;
using System.Threading.Tasks;

namespace _3_parallel_for_foreach_invoke
{
    public class _1_Parallel_For
    {
        public void DoWork()
        {
            SequentialNumbers();
            ParallelNumbers();
        }

        public void DoWorkWithOptions()
        {
            ParallelNumbersWithOptions();
        }

        public void DoWorkWithBreakOrStop()
        {
            ParallelNumbersWithBreakStop();
        }

        private void SequentialNumbers()
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine($"{i} - {Thread.CurrentThread.ManagedThreadId}");
            }
        }

        private void ParallelNumbers()
        {
            int limit = 1000;
            int begin = 0;
            Parallel.For(begin, limit, i =>
            {
                Console.WriteLine($"Parralel {i} - {Thread.CurrentThread.ManagedThreadId}");
            });
        }

        private void ParallelNumbersWithOptions()
        {
            var parallelOpt = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 3
            };

            int limit = 1000;
            int begin = 0;
            Parallel.For(begin, limit, parallelOpt, i =>
            {
                Console.WriteLine($"{i} - {Thread.CurrentThread.ManagedThreadId}");
            });
        }

        private void ParallelNumbersWithBreakStop()
        {
            //Break : complete current iteration item and exit 
            //Stop : stop all iteration items as soon as possible

            //outputs will be different

            var parallelOpt = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 3
            };

            int limit = 1000;
            int begin = 0;
            Parallel.For(begin, limit, parallelOpt, (i, state) =>
            {
                Console.WriteLine($"{i} - {Thread.CurrentThread.ManagedThreadId}");
                if (i > 50)
                {
                    //state.Break();
                    state.Stop();
                    Console.WriteLine("breaked or stopped..");
                    return;
                }
                //if (state.IsStopped) return;
            });
        }
    }
}
