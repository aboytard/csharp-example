namespace TaskParallelLibrary.Data_Parallelism
{
    // use of For example
    public class DirectorySizeExample
    {
        public static void DirectorySizeExampleMain(string[] args)
        {
            long totalSize = 0;

            if (args.Length == 0)
            {
                Console.WriteLine("There are no command line arguments.");
                return;
            }
            if (!Directory.Exists(args[0]))
            {
                Console.WriteLine("The directory does not exist.");
                return;
            }

            string[] files = Directory.GetFiles(args[0]);
            Parallel.For(0, files.Length,
                         index =>
                         {
                             FileInfo fi = new FileInfo(files[index]);
                             long size = fi.Length;
                             Interlocked.Add(ref totalSize, size);
                         });
            Console.WriteLine("Directory '{0}':", args[0]);
            Console.WriteLine("{0:N0} files, {1:N0} bytes", files.Length, totalSize);
        }
    }
}
