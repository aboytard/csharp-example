using static System.Reflection.Metadata.BlobBuilder;
using System.Dynamic;

namespace TaskParallelLibrary.Task_Based_Asynchronous_Programming
{
    class CustomData
    {
        public long CreationTime;
        public int Name;
        public int ThreadNum;
    }

    public class AsyncState
    {    
        // Can use the task.Factory.StartNew when :
        // - Creation and scheduling don't have to be separated and you require additional task creation options or the use of a specific scheduler.
        // - You need to pass additional state into the task that you can retrieve through its Task.AsyncState property.
        public static void AsyncStateMain()
        {
            Task[] taskArray = new Task[10];
            for (int i = 0; i < taskArray.Length; i++)
            {
                taskArray[i] = Task.Factory.StartNew((Object obj) =>
                {
                    CustomData data = obj as CustomData;
                    if (data == null) return;

                    data.ThreadNum = Thread.CurrentThread.ManagedThreadId;
                },
                new CustomData() { Name = i, CreationTime = DateTime.Now.Ticks });
            }
            Task.WaitAll(taskArray);
            foreach (var task in taskArray)
            {
                var data = task.AsyncState as CustomData;
                if (data != null)
                    Console.WriteLine("Task #{0} created at {1}, ran on thread #{2}.",
                                      data.Name, data.CreationTime, data.ThreadNum);
            }
        }

        //If the Result property is accessed before the computation finishes, the property blocks the calling thread until the value is available.
        public static void DoComputationMain()
        {
            Task<Double>[] taskArray = { Task<Double>.Factory.StartNew(() => DoComputation(1.0)),
                                     Task<Double>.Factory.StartNew(() => DoComputation(100.0)),
                                     Task<Double>.Factory.StartNew(() => DoComputation(1000.0)) };

            var results = new Double[taskArray.Length];
            Double sum = 0;

            for (int i = 0; i < taskArray.Length; i++)
            {
                results[i] = taskArray[i].Result;
                Console.Write("{0:N1} {1}", results[i],
                                  i == taskArray.Length - 1 ? "= " : "+ ");
                sum += results[i];
            }
            Console.WriteLine("{0:N1}", sum);
        }

        private static Double DoComputation(Double start)
        {
            Double sum = 0;
            for (var value = start; value <= start + 10; value += .1)
                sum += value;

            return sum;
        }


        //When you use a lambda expression to create a delegate, you have access to all the variables that are visible at that point in your source code.
        //However, in some cases, most notably within loops, a lambda doesn't capture the variable as expected. 
        public static void AsyncStateMain_Issue()
        {
            // Create the task object by using an Action(Of Object) to pass in the loop
            // counter. This produces an unexpected result.
            Task[] taskArray = new Task[10];
            for (int i = 0; i < taskArray.Length; i++)
            {
                taskArray[i] = Task.Factory.StartNew((Object obj) => {
                    var data = new CustomData() { Name = i, CreationTime = DateTime.Now.Ticks };
                    data.ThreadNum = Thread.CurrentThread.ManagedThreadId;
                    Console.WriteLine("Task #{0} created at {1} on thread #{2}.",
                                      data.Name, data.CreationTime, data.ThreadNum);
                },
                i);
            }
            Task.WaitAll(taskArray);
        }

        // use unique identifier for each object in the task
        public static void AsyncStateMain_Issue_Solution()
        {
            // Create the task object by using an Action(Of Object) to pass in custom data
            // to the Task constructor. This is useful when you need to capture outer variables
            // from within a loop.
            Task[] taskArray = new Task[10];
            for (int i = 0; i < taskArray.Length; i++)
            {
                taskArray[i] = Task.Factory.StartNew((Object obj) => {
                    CustomData data = obj as CustomData;
                    if (data == null)
                        return;

                    data.ThreadNum = Thread.CurrentThread.ManagedThreadId;
                    Console.WriteLine("Task #{0} created at {1} on thread #{2}.",
                                     data.Name, data.CreationTime, data.ThreadNum);
                },
                new CustomData() { Name = i, CreationTime = DateTime.Now.Ticks });
            }
            Task.WaitAll(taskArray);
        }

        // use unique identifier for each object in the task
        public static void AsyncStateMain_Issue_Solution_Variant()
        {
            Task[] taskArray = new Task[10];
            for (int i = 0; i < taskArray.Length; i++)
            {
                taskArray[i] = Task.Factory.StartNew((Object obj) =>
                {
                    CustomData data = obj as CustomData;
                    if (data == null) return;

                    data.ThreadNum = Thread.CurrentThread.ManagedThreadId;
                },
                new CustomData() { Name = i, CreationTime = DateTime.Now.Ticks });
            }
            Task.WaitAll(taskArray);
            foreach (var task in taskArray)
            {
                var data = task.AsyncState as CustomData;
                if (data != null)
                    Console.WriteLine("Task #{0} created at {1}, ran on thread #{2}.",
                                      data.Name, data.CreationTime, data.ThreadNum);
            }
        }
    }
}
