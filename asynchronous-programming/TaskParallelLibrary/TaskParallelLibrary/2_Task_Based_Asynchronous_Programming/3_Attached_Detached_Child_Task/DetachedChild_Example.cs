namespace TaskParallelLibrary.Task_Based_Asynchronous_Programming._3_Attached_Detached_Child_Task
{
    public class DetachedChild_Example
    {
        public static void DetachedChild_ExampleMain()
        {
            var parent = Task.Factory.StartNew(() => {
                Console.WriteLine("Outer task executing.");

                var child = Task.Factory.StartNew(() => {
                    Console.WriteLine("Nested task starting.");
                    Thread.SpinWait(500000);
                    Console.WriteLine("Nested task completing.");
                });
            });

            parent.Wait();
            Console.WriteLine("Outer has completed.");
        }

        public static void DetachedChild_ExampleMain_WithTResult()
        {
            var outer = Task<int>.Factory.StartNew(() => {
                Console.WriteLine("Outer task executing.");

                // The Result property blocks until its task completes, as the following example shows.
                var nested = Task<int>.Factory.StartNew(() => {
                    Console.WriteLine("Nested task starting.");
                    Thread.SpinWait(5000000);
                    Console.WriteLine("Nested task completing.");
                    return 42;
                });

                // Parent will wait for this detached child.
                return nested.Result;
            });

            Console.WriteLine("Outer has returned {0}.", outer.Result);
        }
    }
}
