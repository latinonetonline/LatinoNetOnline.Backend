using LatinoNetOnline.Backend.Modules.Events.Core.Services;
using LatinoNetOnline.Backend.Shared.Commons.Extensions;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;
using LatinoNetOnline.Backend.Shared.Infrastructure.Presenter;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Api.Controllers
{
    class ImagesController : BaseController
    {
        private readonly IStorageService _storageService;

        public ImagesController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [AllowAnonymous]
        [HttpPost(Name = "UploadImage")]
        [ProducesResponseType(typeof(OperationResult<Uri>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var result = await _storageService.UploadFile("speakers", Guid.NewGuid().ToString(), file.OpenReadStream().ReadFully());

            return new OperationActionResult(result);
        }
    }
}
