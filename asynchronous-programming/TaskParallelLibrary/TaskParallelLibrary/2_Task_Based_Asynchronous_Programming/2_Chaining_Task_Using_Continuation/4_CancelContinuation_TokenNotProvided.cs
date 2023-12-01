namespace TaskParallelLibrary.Task_Based_Asynchronous_Programming._2_Chaining_Task_Using_Continuation
{
    public class CancelContinuation_TokenNotProvided
    {
        public static async Task CancelContinuation_WithoutTokenMain()
        {
            using var cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            cts.Cancel();

            var task = Task.FromCanceled(token);
            Task continuation =
                task.ContinueWith(
                    antecedent => Console.WriteLine("The continuation is running."),
                    TaskContinuationOptions.NotOnCanceled);

            try
            {
                await task;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
                Console.WriteLine();
            }

            Console.WriteLine($"Task {task.Id}: {task.Status:G}");
            Console.WriteLine($"Task {continuation.Id}: {continuation.Status:G}");
        }
    }
}
