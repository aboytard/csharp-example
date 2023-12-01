using Newtonsoft.Json.Linq;
using RestSharp;

namespace TranslatorApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourceText = "Hello, how are you?";
            string targetLanguageCode = "DE";
            string apiKey = "86e9f3d6-f7e9-d636-738a-e8995f9c1d52:fx";

            var client = new RestClient("https://api-free.deepl.com/v2/translate");
            var request = new RestRequest(Method.POST);
            request.AddParameter("text", sourceText);
            request.AddParameter("target_lang", targetLanguageCode);
            request.AddHeader("Authorization", "DeepL-Auth-Key " + apiKey);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = JObject.Parse(response.Content);
                var translatedText = json["translations"][0]["text"].ToString();
                Console.WriteLine(translatedText);
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusDescription);
            }
        }
    }
}