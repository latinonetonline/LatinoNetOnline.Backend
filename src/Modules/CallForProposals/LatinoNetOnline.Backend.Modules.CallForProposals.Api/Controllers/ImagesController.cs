using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Images;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Services;
using LatinoNetOnline.Backend.Modules.Conferences.Api.Controllers;
using LatinoNetOnline.Backend.Shared.Abstractions.Extensions;
using LatinoNetOnline.Backend.Shared.Infrastructure.Presenter;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Api.Controllers
{
    class ImagesController : BaseController
    {
        private readonly IStorageService _storageService;

        public ImagesController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var result = await _storageService.UploadFile("images", Guid.NewGuid().ToString(), file.OpenReadStream().ReadFully());

            return new OperationActionResult(result);
        }
    }
}
