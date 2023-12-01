using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace _4_asynchronous_programming_othertopics
{

    public interface INameStreamService
    {
        IAsyncEnumerable<string> GetNameList(CancellationToken token = default);
    }

    public class MyNameStreamer : INameStreamService
    {
        public async IAsyncEnumerable<string> GetNameList(CancellationToken token = default)
        {
            using var stream = new StreamReader(File.OpenRead(@"Sample_Lines.txt"));
            await stream.ReadLineAsync();

            string line;

            while ((line = await stream.ReadLineAsync()) != null)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                yield return line;
            }

        }
    }

    public class _4_AsyncStreams
    {
        public async Task DoWork()
        {
            MyNameStreamer streamer = new MyNameStreamer();
            await foreach (var name in streamer.GetNameList())
            {
                Console.WriteLine(name);
            };
        }
    }
}
