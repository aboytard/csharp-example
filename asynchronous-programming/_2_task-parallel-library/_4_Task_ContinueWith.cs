using System;
using System.Threading.Tasks;

namespace _2_task_parallel_library
{
    public class _4_Task_ContinueWith
    {

        public void DoWork1()
        {
            Task.Run(() =>
            {
                Console.WriteLine("Task Started...");
            }).ContinueWith((t) =>
            {
                Console.WriteLine("t Started...");
                for (int i = 0; i < 50; i++)
                {
                    Console.WriteLine("t " + i);
                }
            }, TaskContinuationOptions.OnlyOnRanToCompletion)
            .ContinueWith((t1) =>
             {
                 Console.WriteLine("t1 Started...");
                 int sum = 0;
                 for (int i = 0; i < 50; i++)
                 {
                     Console.WriteLine("t1 " + i);
                     sum += i;
                 }
                 return sum;
             }, TaskContinuationOptions.OnlyOnRanToCompletion)
            .ContinueWith((t2) =>
              {
                  Console.WriteLine("t2 Started...");
                  Console.WriteLine($"t1 result of sum operation is ...{t2.Result}");

              }, TaskContinuationOptions.OnlyOnRanToCompletion)
            .ContinueWith((t3) =>
               {
                   Console.WriteLine("t3 Started...");
                   //operations...
               }, TaskContinuationOptions.LongRunning);

            //OnlyOnRanToCompletion : it means there is no exception and cancelled..
            //canceled : TaskContinuationOptions.OnlyOnCanceled
            //faulted : TaskContinuationOptions.OnlyOnFaulted
            //completion : TaskContinuationOptions.OnlyOnRanToCompletion
        }
    }
}
