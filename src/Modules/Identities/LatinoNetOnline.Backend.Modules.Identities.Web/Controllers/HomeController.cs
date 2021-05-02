using Microsoft.AspNetCore.Mvc;

namespace LatinoNetOnline.Backend.Modules.Identities.Web.Controllers
{
    [Route(BasePath)]
    class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult<string> Get() => "Identity module";
    }
}