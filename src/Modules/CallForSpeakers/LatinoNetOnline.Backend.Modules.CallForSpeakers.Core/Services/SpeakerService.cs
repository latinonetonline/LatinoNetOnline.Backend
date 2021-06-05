using CSharpFunctionalExtensions;

using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Data;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Speakers;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Entities;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Extensions;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Validators;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Services
{
    interface ISpeakerService
    {
        Task<OperationResult<SpeakerDto>> CreateAsync(CreateSpeakerInput input);
        Task<OperationResult<IEnumerable<SpeakerDto>>> GetAllAsync();
    }

    class SpeakerService : ISpeakerService
    {
        private readonly ApplicationDbContext _dbContext;

        public SpeakerService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<OperationResult<IEnumerable<SpeakerDto>>> GetAllAsync()
            => GetAllSpeakersAsync()
                .Map(ConvertToDto)
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

        private Speaker ConvertToEntity(CreateSpeakerInput input) 
            => new(input.Name, input.LastName, input.Email, input.Twitter, input.Description, input.Image);


        private SpeakerDto ConvertToDto(Speaker speaker) 
            => speaker.ConvertToDto();

        private IEnumerable<SpeakerDto> ConvertToDto(IEnumerable<Speaker> speakers) 
            => speakers.Select(x => x.ConvertToDto());


        private async Task<Maybe<IEnumerable<Speaker>>> GetAllSpeakersAsync()
            => await _dbContext.Speakers.AsNoTracking().ToListAsync();

    }
}
