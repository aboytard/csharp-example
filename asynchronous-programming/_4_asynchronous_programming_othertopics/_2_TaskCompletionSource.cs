using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace _4_asynchronous_programming_othertopics
{
    public class _2_TaskCompletionSource
    {

        public Task<string> DoWorkBasic()
        {
            var tcs = new TaskCompletionSource<string>();
            Task.Run(async () =>
            {
                await Task.Delay(5000);
                tcs.SetResult("Jordan");
            });
            return tcs.Task;
        }



        public void DoWork()
        {
            //var source = new TaskCompletionSource<object>();

            //Console.WriteLine($"state 0 : {source.Task.Status}");
            //var process = new Process
            //{
            //    EnableRaisingEvents = true,
            //    StartInfo = new ProcessStartInfo("/Applications/Spotify.app/Contents/MacOS/Spotify")
            //    {
            //        RedirectStandardError = true,
            //        UseShellExecute = false
            //    }
            //};

            //Console.WriteLine($"state 1 : {source.Task.Status}");
            //process.Start();
            //process.Exited += (object sender, EventArgs e) =>
            //{
            //    Console.WriteLine($"state 2 : {source.Task.Status}");
            //    source.SetResult(null);
            //    Console.WriteLine($"state 3 : {source.Task.Status}");
            //};
            //Console.WriteLine($"state 4 : {source.Task.Status}");
        }
    }
}
