namespace TaskParallelLibrary.Task_Based_Asynchronous_Programming._2_Chaining_Task_Using_Continuation
{
    public class Handle_ExceptionThrown
    {
        public static async Task Handle_ExceptionThrownMain_1()
        {
            Task<int> task = Task.Run(
                () =>
                {
                    Console.WriteLine($"Executing task {Task.CurrentId}");
                    return 54;
                });

            var continuation = task.ContinueWith(
                antecedent =>
                {
                    Console.WriteLine($"Executing continuation task {Task.CurrentId}");
                    Console.WriteLine($"Value from antecedent: {antecedent.Result}");

                    throw new InvalidOperationException();
                });

            try
            {
                await task;
                await continuation;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
