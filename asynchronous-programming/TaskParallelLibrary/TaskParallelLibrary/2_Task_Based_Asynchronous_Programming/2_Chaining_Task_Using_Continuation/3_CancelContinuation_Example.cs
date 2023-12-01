namespace TaskParallelLibrary.Task_Based_Asynchronous_Programming._2_Chaining_Task_Using_Continuation
{
    public class CancelContinuationExample
    {
        static readonly Random s_random = new Random((int)DateTime.Now.Ticks);

        public static async Task CancelContinuationExampleMain()
        {
            using var cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            var timer = new Timer(Elapsed, cts, 5000, Timeout.Infinite);

            var task = Task.Run(
                async () =>
                {
                    var product33 = new List<int>();
                    for (int index = 1; index < short.MaxValue; index++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            Console.WriteLine("\nCancellation requested in antecedent...\n");
                            token.ThrowIfCancellationRequested();
                        }
                        if (index % 2000 == 0)
                        {
                            int delay = s_random.Next(16, 501);
                            await Task.Delay(delay);
                        }
                        if (index % 33 == 0)
                        {
                            product33.Add(index);
                        }
                    }

                    return product33.ToArray();
                }, token);

            Task<double> continuation = task.ContinueWith(
                async antecedent =>
                {
                    Console.WriteLine("Multiples of 33:\n");
                    int[] array = antecedent.Result;
                    for (int index = 0; index < array.Length; index++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            Console.WriteLine("\nCancellation requested in continuation...\n");
                            token.ThrowIfCancellationRequested();
                        }
                        if (index % 100 == 0)
                        {
                            int delay = s_random.Next(16, 251);
                            await Task.Delay(delay);
                        }

                        Console.Write($"{array[index]:N0}{(index != array.Length - 1 ? ", " : "")}");

                        if (Console.CursorLeft >= 74)
                        {
                            Console.WriteLine();
                        }
                    }
                    Console.WriteLine();
                    return array.Average();
                }, token).Unwrap();

            try
            {
                await task;
                double result = await continuation;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("\nAntecedent Status: {0}", task.Status);
            Console.WriteLine("Continuation Status: {0}", continuation.Status);
        }

        static void Elapsed(object? state)
        {
            if (state is CancellationTokenSource cts)
            {
                cts.Cancel();
                Console.WriteLine("\nCancellation request issued...\n");
            }
        }
    }
}
