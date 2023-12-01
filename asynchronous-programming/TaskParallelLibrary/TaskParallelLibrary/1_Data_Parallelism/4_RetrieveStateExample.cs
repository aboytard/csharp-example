namespace TaskParallelLibrary.Data_Parallelism
{
    // example on how to retrieve state from a for loop
    public class RetrieveStateExample
    {
        public static void RetrieveStateExampleMain()
        {
            int[] nums = Enumerable.Range(0, 1_000_000).ToArray();
            long total = 0;

            // Use type parameter to make subtotal a long, not an int
            Parallel.For<long>(0, nums.Length, () => 0, (j, loop, subtotal) =>
            {
                subtotal += nums[j];
                return subtotal;
            },
            // interlocked -> to provide information that are shared by different threads
                subtotal => Interlocked.Add(ref total, subtotal)
            );

            Console.WriteLine("The total is {0:N0}", total);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
    // () => 0 --> initialisation
    // In this overload of the method, the third parameter is where you initialize your local state

}
