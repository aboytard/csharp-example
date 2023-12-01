namespace TaskParallelLibrary.Task_Based_Asynchronous_Programming._1_Async_Task_Programming
{
    public class Task_ChildTask
    {
        public static void Task_ChildTaskMain()
        {
            var parent = Task.Factory.StartNew(() => {
                Console.WriteLine("Parent task beginning.");
                for (int ctr = 0; ctr < 10; ctr++)
                {
                    int taskNo = ctr;
                    Task.Factory.StartNew((x) => {
                        Thread.SpinWait(5000000);
                        Console.WriteLine("Attached child #{0} completed.",
                                          x);
                    },
                                          taskNo, TaskCreationOptions.AttachedToParent);
                }
            });

            parent.Wait();
            Console.WriteLine("Parent task completed.");
        }
    }

    // A parent task can use the TaskCreationOptions.DenyChildAttach
    // option to prevent other tasks from attaching to the parent task. 

    // Task.Wait() - Task.WaitAll()
}
