using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LatinoNetOnline.Backend.Modules.Identities.Web.Controllers
{
    [Route(BasePath)]
    class HomeController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<string> Get() => "Identity module";
    }
}