using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace _2_task_parallel_library
{
    public class _5_Task_Cancellation
    {
        private CancellationTokenSource cancellationTokenSource = null;

        public void Run(string name, int number)
        {
            if (cancellationTokenSource != null)
            {
                Console.WriteLine(name + " canceled.");
                cancellationTokenSource.Cancel();
                cancellationTokenSource = null;
                return;
            }
            cancellationTokenSource = new CancellationTokenSource();
            //will run when we cancelled the task
            cancellationTokenSource.Token.Register(() => { Console.WriteLine("i have a cancellation request!"); });
            var list = this.DoWork(number, cancellationTokenSource.Token);
            Console.WriteLine(name + " - " + list.Result.Count);
        }

        private async Task<List<int>> DoWork(int number, CancellationToken token)
        {
            var result = await Task.Run(() =>
            {
                return this.ShowNumbers(number, token);

            }, token);

            return result;
        }

        private List<int> ShowNumbers(int n, CancellationToken token)
        {
            List<int> numbers = new List<int>();
            for (int i = 0; i < n; i++)
            {
                if (token.IsCancellationRequested)
                {
                    return numbers;
                }
                numbers.Add(i);
            }
            return numbers;
        }
    }
}
