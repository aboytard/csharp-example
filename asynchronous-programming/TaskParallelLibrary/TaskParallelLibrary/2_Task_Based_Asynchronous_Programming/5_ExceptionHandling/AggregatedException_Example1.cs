namespace TaskParallelLibrary.Task_Based_Asynchronous_Programming._5_ExceptionHandling
{
    public class AggregatedException_Example1
    {
        public static void Aggregated_Example1()
        {
            var task = Task.Run(
                () => throw new CustomException("This exception is expected!"));

            try
            {
                task.Wait();
            }
            catch (AggregateException ae)
            {
                //Recommendation: do not do catch throw the whole AggregateException because
                //it is analogous to catching the base Exception type in non-parallel scenarios
                foreach (var ex in ae.InnerExceptions)
                {
                    // Handle the custom exception.
                    if (ex is CustomException)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    // Rethrow any other exception.
                    else
                    {
                        throw ex;
                    }
                }
            }
        }

        public static void Aggregated_Example2()
        {
            var task = Task.Run(
                () => throw new CustomException("This exception is expected!"));

            // for example purpose : the while is really unefficient there
            while (!task.IsCompleted) { }

            if (task.Status == TaskStatus.Faulted)
            {
                foreach (var ex in task.Exception?.InnerExceptions ?? new(Array.Empty<Exception>()))
                {
                    // Handle the custom exception.
                    if (ex is CustomException)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    // Rethrow any other exception.
                    else
                    {
                        throw ex;
                    }
                }
            }
        }
    }



    public class CustomException : Exception
    {
        public CustomException(string ex) : base(ex) { }    
    }
}
