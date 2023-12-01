namespace TaskParallelLibrary._0_Asynchronous_Programming_Pattern.Task_based_Asynchronous_Pattern
{
    internal class I_O_BoundTask
    {
        public static Task<DateTimeOffset> Delay(int millisecondsTimeout)
        {
            TaskCompletionSource<DateTimeOffset> tcs = null;
            Timer timer = null;

            timer = new Timer(delegate
            {
                timer.Dispose();
                tcs.TrySetResult(DateTimeOffset.UtcNow);
            }, null, Timeout.Infinite, Timeout.Infinite);

            tcs = new TaskCompletionSource<DateTimeOffset>(timer);
            timer.Change(millisecondsTimeout, Timeout.Infinite);
            return tcs.Task;
        }
        // To create a task that should not be directly
        // backed by a thread for the entirety of its execution, use the TaskCompletionSource<TResult> type
    }
}
