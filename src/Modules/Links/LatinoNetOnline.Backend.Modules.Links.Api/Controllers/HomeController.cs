
using Microsoft.AspNetCore.Mvc;

namespace LatinoNetOnline.Backend.Modules.Links.Api.Controllers
{
    [Route(BasePath)]
    internal class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult<string> Get() => "Links module";
    }
}