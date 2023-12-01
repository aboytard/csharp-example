using System;
using System.Threading.Tasks;

namespace _2_task_parallel_library
{
    public class _1_CreatingAndRunningBasicTasks
    {

        //Please look at the outputs..

        //Start

        public void DoWork1()
        {
            Console.WriteLine("started");
            //started task
            Task.Factory.StartNew(() =>
            {
                int result = 3 + 5;
                Console.WriteLine(result);
            });

            //we are creating a task but do not start.
            var t1 = new Task(() =>
            {
                int result = 5 + 5;
                Console.WriteLine(result);
            });
            //start the task 
            t1.Start();
            Console.WriteLine("completed...");
        }

        //use Wait, Start
        public void DoWork2()
        {
            Console.WriteLine("started DoWork");
            var t1 = Task.Factory.StartNew(() =>
            {
                int result = 3 + 5;
                Console.WriteLine(result);
            });

            //wait until end
            t1.Wait();

            var t2 = new Task(() =>
            {
                int result = 5 + 5;
                Console.WriteLine(result);
            });

            t2.Start();

            //run it synchronously
            //t2.RunSynchronously();

            //wait until end
            t2.Wait();
            Console.WriteLine("completed DoWork...");
        }

    }
}
