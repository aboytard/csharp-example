namespace TaskParallelLibrary._0_Asynchronous_Programming_Pattern.Task_based_Asynchronous_Pattern
{
    static class ReadTask_Manual
    {
        public static Task<int> ReadTask(this Stream stream, byte[] buffer, int offset, int count, object state)
        {
            var tcs = new TaskCompletionSource<int>();
            stream.BeginRead(buffer, offset, count, ar =>
            {
                try { tcs.SetResult(stream.EndRead(ar)); }
                catch (Exception exc) { tcs.SetException(exc); }
            }, state);
            return tcs.Task;
        }
    }

    public class ReadTask_Hybrid
    {
        public Task<int> MethodAsync(string input)
        {
            if (input == null) throw new ArgumentNullException("input");
            return MethodAsyncInternal(input);
        }

        private async Task<int> MethodAsyncInternal(string input)
        {

            // code that uses await goes here
            return 0;
        }

        // Another case where such delegation is useful is when you're
        // implementing fast-path optimization and want to return a cached task.
    }

}
