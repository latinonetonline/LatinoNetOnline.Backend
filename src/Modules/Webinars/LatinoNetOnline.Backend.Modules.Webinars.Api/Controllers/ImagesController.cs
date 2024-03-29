﻿using LatinoNetOnline.Backend.Modules.Webinars.Api.Controllers;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Services;
using LatinoNetOnline.Backend.Shared.Commons.Extensions;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;
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

        [Authorize(Policy = "Anyone")]
        [HttpPost(Name = "UploadImage")]
        [ProducesResponseType(typeof(OperationResult<Uri>), 200)]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var result = await _storageService.UploadFile("images", Guid.NewGuid().ToString() + file.FileName, file.OpenReadStream().ReadFully());

            return new OperationActionResult(result);
        }
    }
}
