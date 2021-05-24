
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LatinoNetOnline.Backend.Modules.Links.Api.Controllers
{
    [Route(BasePath)]
    class HomeController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<string> Get() => "Links module";
    }
}