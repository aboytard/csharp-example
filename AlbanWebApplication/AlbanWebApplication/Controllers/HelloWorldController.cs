using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace AlbanWebApplication.Controllers
{
    public class HelloWorldController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // make the call by adding /HelloWorld/Welcome?name=Rick&numtimes=4 to the URL
        public string Welcome(string name, int numTimes = 1)
        {
            return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}");
        }

        // /HelloWorld/WelcomeWithId/3?coco
        public string WelcomeWithId(string name, int ID = 1)
        {
            return HtmlEncoder.Default.Encode($"Hello {name}, ID: {ID}");
        }

        // title in the view needs to match teh endpoint method
        public IActionResult WelcomeWithAdditionalView(string name, int numTimes = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numTimes;
            return View();
        }
    }
}
