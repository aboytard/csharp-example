namespace TaskParallelLibrary.Task_Based_Asynchronous_Programming._7_Task_Return_Value_Example
{
    public class TaskReturnValueExample
    {
        public static void TaskReturnValueExampleMain()
        {
            // Return a value type with a lambda expression
            Task<int> task1 = Task<int>.Factory.StartNew(() => 1);
            int i = task1.Result;

            // Return a named reference type with a multi-line statement lambda.
            Task<Test> task2 = Task<Test>.Factory.StartNew(() =>
            {
                string s = ".NET";
                double d = 4.0;
                return new Test { Name = s, Number = d };
            });
            Test test = task2.Result;

            // Return an array produced by a PLINQ query
            Task<string[]> task3 = Task<string[]>.Factory.StartNew(() =>
            {
                // get to the picture folder 
                string path = @"C:\";
                string[] files = System.IO.Directory.GetFiles(path);

                var result = (from file in files.AsParallel()
                              let info = new System.IO.FileInfo(file)
                              where info.Extension == ".jpg"
                              select file).ToArray();

                return result;
            });

            foreach (var name in task3.Result)
                Console.WriteLine(name);
        }
    }
    class Test
    {
        public string Name { get; set; }
        public double Number { get; set; }
    }
}
