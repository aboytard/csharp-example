using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _2_task_parallel_library
{
    public class _7_Task_WaitAll
    {
        //output without WaitALL
        //operations started
        //operations completed
        //process 1 :0
        //process 1 :1
        //process 1 :2
        //process 1 :3
        //process 1 :4
        //process 1 :5
        //process 1 :6
        //process 1 :7
        //process 1 :8
        //process 1 :9
        //process 1 :10
        //process 1 :11
        //process 1 :12
        //process 2 :0
        //process 2 :1
        //process 2 :2
        //process 1 :13

        public void DoWork_WaitAll()
        {
            Console.WriteLine("operations started");
            var tasklist = GiveTasks();
            // Wait for the all tasks to complete before 
            Task.WaitAll(tasklist);
            //after all tasks, complete message will be shown
            Console.WriteLine("all operations completed");
        }

        public void DoWork_WaitAny()
        {
            Console.WriteLine("operations started");
            var tasklist = GiveTasks();
            // Wait for the first task to complete,  which one will be completed, not important 
            Task.WaitAny(tasklist);
            //complete message will be shown
            Console.WriteLine("operations completed");
        }

        private Task[] GiveTasks()
        {
            Task t1 = new Task(GiveNumbers);
            Task t2 = new Task(GiveNumbers2);

            t1.Start();
            t2.Start();

            return new Task[] { t1, t2 };
        }


        private void GiveNumbers()
        {
            for (int i = 0; i < 10000; i++)
            {
                Console.WriteLine("process 1 :" + i);
            }
        }

        private void GiveNumbers2()
        {
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine("process 2 :" + i);
            }
        }

    }
}
