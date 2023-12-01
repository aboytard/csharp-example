using System;
using System.Threading.Tasks;

namespace _2_task_parallel_library
{
    public class _3_Task_ActionFunc
    {
        public void DoWorkWithAction()
        {
            Action action = new Action(() =>
            {
                for (int i = 0; i < 50; i++)
                {
                    Console.WriteLine("action : " + i);
                }
            });

            Task mytask = new Task(action);
            mytask.Start();
            //mytask.Wait();
        }

        public int DoWorkWithFunc()
        {
            var myfunc = new Func<int, int>((n) =>
            {
                int sum = 0;
                for (int i = 0; i < n; i++)
                {
                    Task.Delay(500);
                    Console.WriteLine("func : " + i);
                    sum += i;
                }
                return sum;
            });

            Task<int> mytask = new Task<int>(() => { return myfunc(50); });

            mytask.Start();
            mytask.Wait();
            var taskSituation = mytask.Status;
            Console.WriteLine("DoWorkWithFunc task : {0}", mytask.Status);
            if (taskSituation == TaskStatus.RanToCompletion)
            {
                Console.WriteLine("DoWorkWithFunc task completed...");
            }
            return mytask.Result;
        }
    }
}
