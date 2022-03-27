using LatinoNetOnline.Backend.Modules.Events.Core.Entities;
using LatinoNetOnline.Backend.Modules.Events.Core.Services;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;
using LatinoNetOnline.Backend.Shared.Infrastructure.Presenter;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Api.Controllers
{
    class LinksController : BaseController
    {
        private readonly ILinkService _linkService;

        public LinksController(ILinkService linkService)
        {
            _linkService = linkService;
        }

        [AllowAnonymous]
        [HttpGet(Name = "GetLinks")]
        [ProducesResponseType(typeof(OperationResult<Link[]>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
            => new OperationActionResult(await _linkService.GetAllAsync());


        [AllowAnonymous]
        [HttpGet("{name}", Name = "GetLinkByName")]
        [ProducesResponseType(typeof(OperationResult<Link>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string name)
            => new OperationActionResult(await _linkService.GetAsync(name));


        [HttpPost(Name = "CreateLink")]
        [ProducesResponseType(typeof(OperationResult<Link>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(Link model)
            => new OperationActionResult(await _linkService.CreateAsync(model));


        [HttpPut(Name = "UpdateLink")]
        [ProducesResponseType(typeof(OperationResult<Link>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Link model)
            => new OperationActionResult(await _linkService.UpdateAsync(model));


        [HttpDelete(Name = "DeleteLink")]
        [ProducesResponseType(typeof(OperationResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(string name)
            => new OperationActionResult(await _linkService.DeleteAsync(name));

    }
}
