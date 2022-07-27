using Microsoft.AspNetCore.Mvc;

namespace LatinoNetOnline.Backend.Modules.Webinars.Api.Controllers
{
    [ApiVersion("2.0")]
    [ApiVersion("1.0")]
    [ApiController]
    [Route(BasePath + "/[controller]")]
    [Produces("application/json")]
    internal abstract class BaseController : ControllerBase
    {
        protected const string BasePath = "api/v{version:apiVersion}/webinars-module";
    }
}