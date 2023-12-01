namespace TaskParallelLibrary.Task_Based_Asynchronous_Programming._5_ExceptionHandling
{
    public class Flatten_NestedException_Example
    {
        public static void Flatten_NestedException_Example1()
        {
            var task = Task.Factory.StartNew(() =>
            {
                var child = Task.Factory.StartNew(() =>
                {
                    var grandChild = Task.Factory.StartNew(() =>
                    {
                        // This exception is nested inside three AggregateExceptions.
                        throw new CustomException("Attached child2 faulted.");
                    }, TaskCreationOptions.AttachedToParent);

                    // This exception is nested inside two AggregateExceptions.
                    throw new CustomException("Attached child1 faulted.");
                }, TaskCreationOptions.AttachedToParent);
            });

            try
            {
                task.Wait();
            }
            catch (AggregateException ae)
            {
                foreach (var ex in ae.Flatten().InnerExceptions)
                {
                    if (ex is CustomException)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public static void TaskExceptionTwo()
        {
            try
            {
                Directory_task_Exception_exampleExecuteTasks();
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    Console.WriteLine(
                        "{0}:\n   {1}", e.GetType().Name, e.Message);
                }
            }
        }

        public static void Directory_task_Exception_exampleExecuteTasks()
        {
            // Assume this is a user-entered String.
            string path = @"C:\";
            List<Task> tasks = new();

            tasks.Add(Task.Run(() =>
            {
                // This should throw an UnauthorizedAccessException.
                return Directory.GetFiles(
                    path, "*.txt",
                    SearchOption.AllDirectories);
            }));

            tasks.Add(Task.Run(() =>
            {
                if (path == @"C:\")
                {
                    throw new ArgumentException(
                        "The system root is not a valid path.");
                }
                return new string[] { ".txt", ".dll", ".exe", ".bin", ".dat" };
            }));

            tasks.Add(Task.Run(() =>
            {
                throw new NotImplementedException(
                    "This operation has not been implemented.");
            }));

            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                throw ae.Flatten();
            }
        }
    }
}