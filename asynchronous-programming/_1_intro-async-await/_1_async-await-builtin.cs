using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace _1_intro_async_await
{
    public class MyFileReader
    {
        //await samples with built in methods
        public async Task<string> ReadToEndFromFile(string path)
        {
            string result;
            using (StreamReader sr = new StreamReader(File.OpenRead(path)))
            {
                result = await sr.ReadToEndAsync();
            }
            return result;
        }


        public async Task<List<string>> ReadLineFromFile(string path)
        {
            List<string> lines = new List<string>();

            using (StreamReader sr = new StreamReader(File.OpenRead(path)))
            {
                while (true)
                {
                    string line = await sr.ReadLineAsync();
                    if (line == null)
                    {
                        break;
                    }
                    lines.Add(line);
                }
            }
            return lines;
        }

        //BEST PRACTICE !!
        //avoid to use async void combination. use async Task combination
        public async Task GetFileWebClient(string uri)
        {
            string response;
            using (HttpClient client = new HttpClient())
            {
                response = await client.GetStringAsync(uri);
            }
            Console.WriteLine(response);
        }

    }
}
