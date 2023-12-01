namespace TaskParallelLibrary.Task_Based_Asynchronous_Programming._1_Async_Task_Programming
{
    public class Task_CreateOptions
    {
        // Specifies flags that control optional behavior for the creation and execution of tasks.
        public static void Task_CreateOptionsMain()
        {
            var task3 = new Task(() => Console.Write("This is supposed to be a long task..."),
                                TaskCreationOptions.LongRunning | TaskCreationOptions.PreferFairness);
                    task3.Start();
        }
    }
}
