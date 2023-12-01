namespace TaskParallelLibrary.Task_Based_Asynchronous_Programming._1_Async_Task_Programming
{
    // When user code that's running in a task creates a new task and doesn't specify the AttachedToParent option,
    // the new task isn't synchronized with the parent task in any special way.
    public class Task_CreatingDetachedChild
    {
        public static void Task_CreatingDetachedChildMain()
        {
            var outer = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Outer task beginning.");

                var child = Task.Factory.StartNew(() =>
                {
                    Thread.SpinWait(5000000);
                    Console.WriteLine("Detached task completed.");
                });
            });

            outer.Wait();
            Console.WriteLine("Outer task completed.");
        }
    }
}
