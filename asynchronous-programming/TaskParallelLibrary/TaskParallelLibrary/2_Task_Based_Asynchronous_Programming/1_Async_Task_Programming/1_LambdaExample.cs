namespace TaskParallelLibrary.Task_Based_Asynchronous_Programming
{
    public class LambdaExample
    {
        public static void LambdaExampleMain_Start()
        {
            Thread.CurrentThread.Name = "Main";

            // Create a task and supply a user delegate by using a lambda expression.
            Task taskA = new Task(() => Console.WriteLine("Hello from taskA."));
            // Start the task.
            taskA.Start();

            // Output a message from the calling thread.
            Console.WriteLine("Hello from thread '{0}'.",
                              Thread.CurrentThread.Name);
            taskA.Wait();
        }

        public static void LambdaExampleMain_Run()
        {
            Thread.CurrentThread.Name = "Main";

            // Define and run the task.
            Task taskA = Task.Run(() => Console.WriteLine("Hello from taskA."));

            // Output a message from the calling thread.
            Console.WriteLine("Hello from thread '{0}'.",
                                Thread.CurrentThread.Name);
            taskA.Wait();
        }
    }
}
