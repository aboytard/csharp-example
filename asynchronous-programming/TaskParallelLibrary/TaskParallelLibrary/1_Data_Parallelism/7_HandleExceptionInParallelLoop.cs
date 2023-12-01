using System.Collections.Concurrent;

namespace TaskParallelLibrary.Data_Parallelism
{
    public class HandleExceptionInParallelLoopExample
    {
        public static void HandleExceptionInParallelLoopExampleMain()
        {
            // Create some random data to process in parallel.
            // There is a good probability this data will cause some exceptions to be thrown.
            byte[] data = new byte[5_000];
            Random r = Random.Shared;
            r.NextBytes(data);

            try
            {
                ProcessDataInParallel(data);
            }
            catch (AggregateException ae)
            {
                var ignoredExceptions = new List<Exception>();
                // This is where you can choose which exceptions to handle.
                foreach (var ex in ae.Flatten().InnerExceptions)
                {
                    if (ex is ArgumentException) Console.WriteLine(ex.Message);
                    else ignoredExceptions.Add(ex);
                }
                if (ignoredExceptions.Count > 0)
                {
                    throw new AggregateException(ignoredExceptions);
                }
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static void ProcessDataInParallel(byte[] data)
        {
            // Use ConcurrentQueue to enable safe enqueueing from multiple threads.
            var exceptions = new ConcurrentQueue<Exception>();

            // Execute the complete loop and capture all exceptions.
            Parallel.ForEach(data, d =>
            {
                try
                {
                    // Cause a few exceptions, but not too many.
                    if (d < 3) throw new ArgumentException($"Value is {d}. Value must be greater than or equal to 3.");
                    else Console.Write(d + " ");
                }
                // Store the exception and continue with the loop.
                catch (Exception e)
                {
                    exceptions.Enqueue(e);
                }
            });
            Console.WriteLine();

            // Throw the exceptions here after the loop completes.
            if (!exceptions.IsEmpty)
            {
                throw new AggregateException(exceptions);
            }
        }
    }
}
