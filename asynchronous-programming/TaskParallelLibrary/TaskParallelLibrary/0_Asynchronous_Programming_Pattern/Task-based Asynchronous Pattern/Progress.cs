using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskParallelLibrary._0_Asynchronous_Programming_Pattern.Task_based_Asynchronous_Pattern
{
    internal class Progress
    {
        public async Task DoWork()
        {
            var myProgress = new Progress<int>(ReportProgress);
            int myNumbers = await CountNumbers(GetNumbers(), myProgress);

        }

        void ReportProgress(int value)
        {
            Console.WriteLine($"percent: {value}");
        }

        private async Task<int> CountNumbers(List<int> numbers, IProgress<int> progress)
        {
            int totalCount = numbers.Count;
            int completedNumber = await Task.Run<int>(async () =>
            {

                int count = 0;
                foreach (var num in numbers)
                {
                    var powNumber = await Calculate(num);

                    count++;

                    if (progress != null)
                    {
                        progress.Report(((count * 100) / totalCount));
                    }
                }

                return count;

            });

            return completedNumber;
        }

        private Task<double> Calculate(int number)
        {
            return Task.Factory.StartNew<double>(() =>
            {
                return Math.Pow(number, 2);
            });
        }

        private List<int> GetNumbers()
        {
            var numbers = Enumerable.Range(1, 1000).ToList<int>();
            return numbers;
        }
    }
}
