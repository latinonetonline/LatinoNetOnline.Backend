using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LatinoNetOnline.Backend.Modules.Events.Api.Controllers
{
    [ApiVersion("2.0")]
    [ApiVersion("1.0")]
    [ApiController]
    [Route(BasePath + "/[controller]")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(OperationResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    internal abstract class BaseController : ControllerBase
    {
        protected const string BasePath = "api/v{version:apiVersion}/events-module";
    }
}