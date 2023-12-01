namespace TaskParallelLibrary.Task_Based_Asynchronous_Programming._2_Chaining_Task_Using_Continuation
{
    public class Continuation_ReturningTaskType
    {
        public static async Task Continuation_ReturningTaskTypeMain()
        {
            Task<int> taskOne = RemoteIncrement(0);
            Console.WriteLine("Started RemoteIncrement(0)");

            Task<int> taskTwo = RemoteIncrement(4)
                .ContinueWith(t => RemoteIncrement(t.Result))
                .Unwrap().ContinueWith(t => RemoteIncrement(t.Result))
                .Unwrap().ContinueWith(t => RemoteIncrement(t.Result))
                .Unwrap();

            Console.WriteLine("Started RemoteIncrement(...(RemoteIncrement(RemoteIncrement(4))...)");

            try
            {
                await taskOne;
                Console.WriteLine("Finished RemoteIncrement(0)");

                await taskTwo;
                Console.WriteLine("Finished RemoteIncrement(...(RemoteIncrement(RemoteIncrement(4))...)");
            }
            catch (Exception e)
            {
                Console.WriteLine($"A task has thrown the following (unexpected) exception:\n{e}");
            }
        }

        static Task<int> RemoteIncrement(int number) =>
            Task<int>.Factory.StartNew(
                obj =>
                {
                    Thread.Sleep(1000);

                    int x = (int)(obj!);
                    Console.WriteLine("Thread={0}, Next={1}", Thread.CurrentThread.ManagedThreadId, ++x);
                    return x;
                },
                number);
    }
}
