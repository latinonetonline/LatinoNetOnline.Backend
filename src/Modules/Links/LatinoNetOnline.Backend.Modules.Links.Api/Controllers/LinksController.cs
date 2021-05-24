using LatinoNetOnline.Backend.Modules.Links.Core.Entities;
using LatinoNetOnline.Backend.Shared.Infrastructure.Presenter;

using LatinoNETOnline.App.Infrastructure.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Links.Api.Controllers
{
    class LinksController : BaseController
    {
        private readonly ILinkService _linkService;

        public LinksController(ILinkService linkService)
        {
            _linkService = linkService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => new OperationActionResult(await _linkService.GetAll());


        [AllowAnonymous]
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
            => new OperationActionResult(await _linkService.Get(name));


        [HttpPost]
        public async Task<IActionResult> Create(Link model)
            => new OperationActionResult(await _linkService.Create(model));


        [HttpPut]
        public async Task<IActionResult> Update(Link model)
            => new OperationActionResult(await _linkService.Update(model));


        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
            => new OperationActionResult(await _linkService.Delete(name));

    }
}
