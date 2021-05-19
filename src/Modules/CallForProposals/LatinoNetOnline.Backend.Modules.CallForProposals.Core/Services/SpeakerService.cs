using CSharpFunctionalExtensions;

using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Data;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Speakers;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Entities;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Extensions;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Validators;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Services
{
    interface ISpeakerService
    {
        Task<OperationResult<SpeakerDto>> CreateAsync(CreateSpeakerInput input);
        Task<OperationResult<IEnumerable<Speaker>>> GetAllAsync();
    }

    class SpeakerService : ISpeakerService
    {
        private readonly ApplicationDbContext _dbContext;

        public SpeakerService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<OperationResult<IEnumerable<Speaker>>> GetAllAsync()
            => GetAllSpeakersAsync()
                .ToResult("No hay ningún speaker.")
                .FinallyOperationResult();


        public Task<OperationResult<SpeakerDto>> CreateAsync(CreateSpeakerInput input)
            => Validate(input)
                .Map(input => ConvertToEntity(input))
                .Map(speaker => AddSpeakerAsync(speaker))
                .Map(speaker => ConvertToDto(speaker))
                .FinallyOperationResult();



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

        private Speaker ConvertToEntity(CreateSpeakerInput input) => new()
        {
            Name = input.Name,
            LastName = input.LastName,
            Email = input.Email,
            Description = input.Description,
            Twitter = input.Twitter,
            Image = input.Image
        };

        private SpeakerDto ConvertToDto(Speaker speaker) => speaker.ConvertToDto();


        private async Task<Maybe<IEnumerable<Speaker>>> GetAllSpeakersAsync()
            => await _dbContext.Speakers.AsNoTracking().ToListAsync();

    }
}
