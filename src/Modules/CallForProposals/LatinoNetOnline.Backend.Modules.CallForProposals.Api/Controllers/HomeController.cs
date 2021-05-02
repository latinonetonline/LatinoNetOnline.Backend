using Microsoft.AspNetCore.Mvc;

namespace LatinoNetOnline.Backend.Modules.Conferences.Api.Controllers
{
    [Route(BasePath)]
    internal class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult<string> Get() => "Call For Proposals module";
    }
}