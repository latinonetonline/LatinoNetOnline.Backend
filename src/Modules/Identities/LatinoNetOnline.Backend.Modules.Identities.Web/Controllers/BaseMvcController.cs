using Microsoft.AspNetCore.Mvc;

namespace LatinoNetOnline.Backend.Modules.Identities.Web.Controllers
{
    [Route(BasePath + "/[controller]/[action]")]
    abstract class BaseMvcController : Controller
    {
        protected const string BasePath = "identities-module";
    }
}