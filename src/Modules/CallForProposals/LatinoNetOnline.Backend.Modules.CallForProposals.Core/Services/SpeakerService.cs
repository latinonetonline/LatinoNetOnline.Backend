using CSharpFunctionalExtensions;

using Microsoft.EntityFrameworkCore;

using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Data;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Speakers;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Entities;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Extensions;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Validators;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Services
{
    interface ISpeakerService
    {
        Task<Result<SpeakerDto>> CreateAsync(CreateSpeakerInput input);
        Task<List<Speaker>> GetAllAsync();
    }

    class SpeakerService : ISpeakerService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IStorageService _storageService;

        public SpeakerService(ApplicationDbContext dbContext, IStorageService storageService)
        {
            _dbContext = dbContext;
            _storageService = storageService;
        }

        public Task<List<Speaker>> GetAllAsync()
        {
            return _dbContext.Speakers.ToListAsync();
        }

        public async Task<Result<SpeakerDto>> CreateAsync(CreateSpeakerInput input)
        {
            return await Validate(input)
                .Map(input => GetImageLinkAsync(input.Image).ToResult("No se pudo subir la imagen."))
                .Map(image => ConvertToEntity(input, image))
                .Map(speaker => AddSpeakerAsync(speaker))
                .Map(speaker => ConvertToDto(speaker));

        }

        private Result<CreateSpeakerInput> Validate(CreateSpeakerInput input)
        {
            CreateSpeakerValidator validator = new();

            var validationResult = validator.Validate(input);

            return validationResult.ToResult(input);

        }

        private async Task<Speaker> AddSpeakerAsync(Speaker speaker)
        {
            await _dbContext.Speakers.AddAsync(speaker);
            await _dbContext.SaveChangesAsync();

            return speaker;
        }

        private Speaker ConvertToEntity(CreateSpeakerInput input, Uri image) => new()
        {
            Name = input.Name,
            LastName = input.LastName,
            Email = input.Email,
            Description = input.Description,
            Twitter = input.Twitter,
            Image = image
        };

        private SpeakerDto ConvertToDto(Speaker speaker) => speaker.ConvertToDto();

        private async Task<Maybe<Uri>> GetImageLinkAsync(byte[] image)
            => await _storageService.UploadFile("images", Guid.NewGuid().ToString(), image);

    }
}
