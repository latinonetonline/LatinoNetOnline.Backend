using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Api.Controllers
{
    [Route(BasePath)]
    internal class HomeController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<string> Get() => "Call For Speakers module";
    }
}