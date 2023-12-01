using System;
using System.Threading.Tasks;

namespace _2_task_parallel_library
{
    public class _6_Task_Handling_Exceptions
    {
        public void DoWork()
        {
            Task t1 = new Task(() =>
            {
                for (int i = 0; i < 100000; i++)
                {
                    if (i > 5000)
                    {
                        throw new InvalidOperationException("my exception");
                    }
                    Console.WriteLine(i);
                }
            });

            //potencial exception will be fired
            try
            {
                t1.Start();
                t1.Wait();
            }
            catch (AggregateException ex)
            {
                //approach 1
                ex.Handle(agg =>
                {
                    Console.WriteLine(ex.Message);
                    return true;
                });

                //approach 2
                //foreach (var e in ex.Flatten().InnerExceptions)
                //{
                //    if (e is Exception)
                //    {
                //        Console.WriteLine(e.Message);
                //    }
                //}
            }

        }
    }
}
