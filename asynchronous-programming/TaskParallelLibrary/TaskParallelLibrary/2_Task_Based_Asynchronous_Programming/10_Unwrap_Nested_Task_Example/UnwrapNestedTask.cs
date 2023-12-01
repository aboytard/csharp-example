namespace TaskParallelLibrary.Task_Based_Asynchronous_Programming._10_Unwrap_Nested_Task_Example
{
    public class UnwrapNestedTask
    {
        static void UnwrapNestedTaskMain()
        {
            // An arbitrary threshold value.
            byte threshold = 0x40;

            // data is a Task<byte[]>
            var data = Task<byte[]>.Factory.StartNew(() =>
            {
                return GetData();
            });

            // We want to return a task so that we can
            // continue from it later in the program.
            // Without Unwrap: stepTwo is a Task<Task<byte[]>>
            // With Unwrap: stepTwo is a Task<byte[]>
            var stepTwo = data.ContinueWith((antecedent) =>
            {
                return Task<byte>.Factory.StartNew(() => Compute(antecedent.Result));
            })
                .Unwrap();

            // Without Unwrap: antecedent.Result = Task<byte>
            // and the following method will not compile.
            // With Unwrap: antecedent.Result = byte and
            // we can work directly with the result of the Compute method.
            var lastStep = stepTwo.ContinueWith((antecedent) =>
            {
                if (antecedent.Result >= threshold)
                {
                    return Task.Factory.StartNew(() => Console.WriteLine("Program complete. Final = 0x{0:x} threshold = 0x{1:x}", stepTwo.Result, threshold));
                }
                else
                {
                    return DoSomeOtherAsynchronousWork(stepTwo.Result, threshold);
                }
            });

            lastStep.Wait();
            Console.WriteLine("Press any key");
            Console.ReadKey();
        }

        #region Dummy_Methods
        private static byte[] GetData()
        {
            Random rand = new Random();
            byte[] bytes = new byte[64];
            rand.NextBytes(bytes);
            return bytes;
        }

        static Task DoSomeOtherAsynchronousWork(int i, byte b2)
        {
            return Task.Factory.StartNew(() =>
            {
                Thread.SpinWait(500000);
                Console.WriteLine("Doing more work. Value was <= threshold");
            });
        }
        static byte Compute(byte[] data)
        {

            byte final = 0;
            foreach (byte item in data)
            {
                final ^= item;
                Console.WriteLine("{0:x}", final);
            }
            Console.WriteLine("Done computing");
            return final;
        }
        #endregion
    }
}
}
