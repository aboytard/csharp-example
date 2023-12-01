using System;
using System.Threading.Tasks;
using System.Threading;

namespace _4_asynchronous_programming_othertopics
{
    public class _5_Interlocked_Cancellation
    {

        public void DoWork()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            //after 10 ms, it will be cancelled. everytime you can get different totalNumber value. 
            cancellationTokenSource.CancelAfter(10);
            ParallelOptions options = new ParallelOptions()
            {
                CancellationToken = cancellationTokenSource.Token,
                MaxDegreeOfParallelism = 1
            };

            int totalNumber = 0;

            try
            {
                Random rnd = new Random();
                //after cancellation you will get an exception OperationCancelledException, so we will add try catch
                Parallel.For(0, 1000000, options, (i) =>
                {
                    //if you will work with integer, choose the Interlocked for atomic operations
                    // it is faster than lock() keyword
                    Interlocked.Add(ref totalNumber, i);

                });
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine("cancelled");
            }
            Console.WriteLine($"total is : {totalNumber}");

        }



    }
}
