using System;
using System.Threading.Tasks;

namespace _2_task_parallel_library
{
    public class _2_TaskRun
    {
        //without waiting for each other 
        public Task<string[]> GetLines(string path)
        {
            Console.WriteLine("started");
            var result = Task.Run<string[]>(() =>
            {
                var lines = System.IO.File.ReadAllLines(path);
                return lines;
            });
            Console.WriteLine("GetLines task : {0}", result.Status);
            Console.WriteLine("ended");
            return result;
        }

        //with waiting for each other 
        public async Task<string[]> GetLines2(string path)
        {
            Console.WriteLine("started");
            var result = await Task.Run(() =>
            {
                var lines = System.IO.File.ReadAllLines(path);
                return lines;
            });

            Console.WriteLine("ended");
            return result;
        }

        //Return Value Task
        public string DoWorkWithReturn()
        {
            Task<string> t1 = Task.Run(() => { return ReturnName(); });
            return t1.Result;
        }

        private string ReturnName()
        {
            return "Hello";
        }
    }
}
