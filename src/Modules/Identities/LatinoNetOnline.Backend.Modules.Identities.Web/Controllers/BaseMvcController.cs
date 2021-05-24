using Microsoft.AspNetCore.Mvc;

namespace LatinoNetOnline.Backend.Modules.Identities.Web.Controllers
{
    [Route(BasePath + "/[controller]/[action]")]
    abstract class BaseMvcController : Controller
    {
        public const string BasePath = "identities-module";
    }
}