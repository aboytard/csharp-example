namespace TaskParallelLibrary.Task_Based_Asynchronous_Programming._3_Attached_Detached_Child_Task
{
    public class ParentDenyingChildAttachingAccess_Example
    {
        public static void ParentDenyingChildAttachingAccess_ExampleMain()
        {
            var parent = Task.Run(() => {
                Console.WriteLine("Parent task executing.");
                var child = Task.Factory.StartNew(() => {
                    Console.WriteLine("Attached child starting.");
                    Thread.SpinWait(5000000);
                    Console.WriteLine("Attached child completing.");
                }, TaskCreationOptions.AttachedToParent);
            });
            parent.Wait();
            Console.WriteLine("Parent has completed.");
        }
    }
}
