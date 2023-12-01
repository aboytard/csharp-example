using System.Collections.Concurrent;

namespace TaskParallelLibrary.Data_Parallelism
{
    public class SpeedUpSmallLoopBodiesExample
    {
        public static void SpeedUpSmallLoopBodiesExampleMain()
        {
            // Source must be array or IList.
            var source = Enumerable.Range(0, 100000).ToArray();

            // Partition the entire source array.
            var rangePartitioner = Partitioner.Create(0, source.Length);

            double[] results = new double[source.Length];

            // Loop over the partitions in parallel.
            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                // Loop over each range element without a delegate invocation.
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    results[i] = source[i] * Math.PI;
                }
            });

            Console.WriteLine("Operation complete. Print results? y/n");
            char input = Console.ReadKey().KeyChar;
            if (input == 'y' || input == 'Y')
            {
                foreach (double d in results)
                {
                    Console.Write("{0} ", d);
                }
            }
        }
    }
}
