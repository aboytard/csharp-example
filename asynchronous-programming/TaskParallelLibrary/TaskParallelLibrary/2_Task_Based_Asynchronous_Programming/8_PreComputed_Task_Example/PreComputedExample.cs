﻿using System.Collections.Concurrent;
using System.Diagnostics;

namespace TaskParallelLibrary.Task_Based_Asynchronous_Programming._8_PreComputed_Task_Example
{
    public class PreComputedExample
    {
        private static readonly ConcurrentDictionary<string, string> s_cachedDownloads = new();
        private static readonly HttpClient s_httpClient = new();

        public static Task<string> DownloadStringAsync(string address)
        {
            if (s_cachedDownloads.TryGetValue(address, out string? content))
            {
                return Task.FromResult(content);
            }

            return Task.Run(async () =>
            {
                content = await s_httpClient.GetStringAsync(address);
                s_cachedDownloads.TryAdd(address, content);

                return content;
            });
        }

        public static async Task PreComputedExampleMain()
        {
            string[] urls = new[]
            {
            "https://learn.microsoft.com/aspnet/core",
            "https://learn.microsoft.com/dotnet",
            "https://learn.microsoft.com/dotnet/architecture/dapr-for-net-developers",
            "https://learn.microsoft.com/dotnet/azure",
            "https://learn.microsoft.com/dotnet/desktop/wpf",
            "https://learn.microsoft.com/dotnet/devops/create-dotnet-github-action",
            "https://learn.microsoft.com/dotnet/machine-learning",
            "https://learn.microsoft.com/xamarin",
            "https://dotnet.microsoft.com/",
            "https://www.microsoft.com"
        };

            Stopwatch stopwatch = Stopwatch.StartNew();
            IEnumerable<Task<string>> downloads = urls.Select(DownloadStringAsync);


            await Task.WhenAll(downloads).ContinueWith(
                downloadTasks => StopAndLogElapsedTime(1, stopwatch, downloadTasks));

            // Perform the same operation a second time. The time required
            // should be shorter because the results are held in the cache.
            stopwatch.Restart();

            downloads = urls.Select(DownloadStringAsync);

            await Task.WhenAll(downloads).ContinueWith(
                downloadTasks => StopAndLogElapsedTime(2, stopwatch, downloadTasks));
        }

        static void StopAndLogElapsedTime(
            int attemptNumber, Stopwatch stopwatch, Task<string[]> downloadTasks)
        {
            stopwatch.Stop();

            int charCount = downloadTasks.Result.Sum(result => result.Length);
            long elapsedMs = stopwatch.ElapsedMilliseconds;

            Console.WriteLine(
                $"Attempt number: {attemptNumber}\n" +
                $"Retrieved characters: {charCount:#,0}\n" +
                $"Elapsed retrieval time: {elapsedMs:#,0} milliseconds.\n");
        }

    }
}
