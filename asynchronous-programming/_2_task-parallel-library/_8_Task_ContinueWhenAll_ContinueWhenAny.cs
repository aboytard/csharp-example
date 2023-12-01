using System;
using System.Threading.Tasks;

namespace _2_task_parallel_library
{
    public class _8_Task_ContinueWhenAll
    {

        public void DoWork()
        {
            Task[] myTasks = new Task[2];
            myTasks[0] = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 500; i++)
                {
                    Console.WriteLine("task 1 : {0}", i);
                }
            });

            myTasks[1] = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 50; i++)
                {
                    Console.WriteLine("task 2 : {0}", i);
                }
            });


            Task.Factory.ContinueWhenAll(myTasks, completed =>
            {
                Console.WriteLine("all of them completed...");
            });

            //Task.Factory.ContinueWhenAny(myTasks, completed =>
            //{
            //    Console.WriteLine("some of them completed...");
            //});

        }

    }
}
