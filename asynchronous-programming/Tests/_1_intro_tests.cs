using System;
using System.Net.Http;
using System.Threading.Tasks;
using _1_intro_async_await;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class IntroAsyncTests
    {

        [TestMethod]
        public async Task ReadToEndTest()
        {
            MyFileReader reader = new MyFileReader();
            var result = await reader.ReadToEndFromFile("C:/Test-Sample/Asynchronous/Sample.txt");
            Console.WriteLine(result);
            Assert.AreEqual(44, result.Length);
        }

        [TestMethod]
        public async Task ReadLineTest()
        {
            MyFileReader reader = new MyFileReader();
            var list = await reader.ReadLineFromFile("C:/Test-Sample/Asynchronous/Sample.txt");

            Console.WriteLine(list.Count);
            Assert.AreEqual(1, list.Count);
        }

        [TestMethod]
        public async Task GetAsyncTestAsync()
        {
            string response;
            using (HttpClient client = new HttpClient())
            {
                response = await client.GetStringAsync("http://developer.db.com");
            }
            Console.WriteLine(response);

            Assert.AreEqual(true, response.Length > 0);
        }

        [TestMethod]
        public async Task GetAsyncTest()
        {
            MyFileReader reader = new MyFileReader();
            try
            {
                await reader.GetFileWebClient("http://developer.db.com");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
