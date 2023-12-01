using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskParallelLibrary.Task_Based_Asynchronous_Programming._2_Chaining_Task_Using_Continuation
{
    // also example to use the AsyncState
    public class AssociateState_To_Continuation_Example
    {
        static DateTime DoWork()
        {
            Thread.Sleep(2000);

            return DateTime.Now;
        }

        public static async Task AssociateState_To_Continuation_ExampleMain()
        {
            Task<DateTime> task = Task.Run(() => DoWork());

            var continuations = new List<Task<DateTime>>();
            for (int i = 0; i < 5; i++)
            {
                task = task.ContinueWith((antecedent, _) => DoWork(), DateTime.Now);
                continuations.Add(task);
            }

            await task;

            foreach (Task<DateTime> continuation in continuations)
            {
                DateTime start = (DateTime)continuation.AsyncState!;
                DateTime end = continuation.Result;

                Console.WriteLine($"Task was created at {start.TimeOfDay} and finished at {end.TimeOfDay}.");
            }

            Console.ReadLine();
        }
    }
}
