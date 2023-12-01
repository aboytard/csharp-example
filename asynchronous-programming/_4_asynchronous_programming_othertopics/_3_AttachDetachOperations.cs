using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace _4_asynchronous_programming_othertopics
{
    public class _3_AttachDetachOperations
    {
        public async Task DoWork()
        {
            Console.WriteLine("Operations Started");
            //default TaskCreationOptions is DenyChildAttach

            //AttachedToParemt - parent waits for all child task complete.
            await Task.Factory.StartNew(() =>
            {
                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("task 1 started");
                    WriteNumbers(5);
                    Console.WriteLine("task 1 completed");
                }, TaskCreationOptions.AttachedToParent);

                Task.Factory.StartNew(() =>
                 {
                     Console.WriteLine("task 2 started");
                     WriteNumbers(7);
                     Console.WriteLine("task 2 completed");
                 }, TaskCreationOptions.AttachedToParent);

                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("task 3 started");
                    WriteNumbers(8);
                    Console.WriteLine("task 3 completed");
                }, TaskCreationOptions.AttachedToParent);
            });
            Console.WriteLine("Operations Completed");
        }

        private void WriteNumbers(int limit)
        {
            for (int i = 0; i < limit; i++)
            {
                Console.WriteLine(i);
            }
        }

        public void DoWork2()
        {
            var parent = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("task starting.");
                var child = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Attached child starting.");
                    Thread.SpinWait(5000000);
                    Console.WriteLine("Attached child completing.");
                }, TaskCreationOptions.AttachedToParent);
            });
            parent.Wait();
            Console.WriteLine("all completed.");
        }
    }
}
