using Microsoft.AspNetCore.Mvc;

namespace MyServiceApi
{
    [ApiController]
    [Route("/myServiceApi/")]
    public class MyServiceApi : ControllerBase
    {
        private MyService _myService { get; set; }
        public MyServiceApi(MyService myService) 
        {
            _myService = myService;
        }

        [HttpGet("test")]
        public IActionResult Get()
        {
            return Ok(_myService.TestMethod());
        }

    }
}
