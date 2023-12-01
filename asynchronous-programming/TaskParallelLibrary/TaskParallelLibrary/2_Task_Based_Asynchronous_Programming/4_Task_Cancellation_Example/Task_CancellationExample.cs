namespace TaskParallelLibrary.Task_Based_Asynchronous_Programming._4_Task_Cancellation_Example
{
    public class Task_CancellationExample
    {
        public static async Task Task_CancellationExampleMain()
        {
            var tokenSource2 = new CancellationTokenSource();
            CancellationToken ct = tokenSource2.Token;

            var task = Task.Run(() =>
            {
                // Were we already canceled?
                ct.ThrowIfCancellationRequested();

                bool moreToDo = true;
                while (moreToDo)
                {
                    // Poll on this property if you have to do
                    // other cleanup before throwing.
                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        ct.ThrowIfCancellationRequested();
                    }
                }
            }, tokenSource2.Token); // Pass same token to Task.Run.

            tokenSource2.Cancel();

            // Just continue on this thread, or await with try-catch:
            try
            {
                await task;
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
            }
            finally
            {
                tokenSource2.Dispose();
            }

            Console.ReadKey();
        }
    }
}
