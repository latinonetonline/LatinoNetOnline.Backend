using CSharpFunctionalExtensions;

using LatinoNetOnline.Backend.Modules.Webinars.Core.Data;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Speakers;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Extensions;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Validators;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Services
{
    interface ISpeakerService
    {
        Task<OperationResult<SpeakerDto>> CreateAsync(CreateSpeakerInput input);
        Task<OperationResult<IEnumerable<SpeakerDto>>> GetAllAsync();
        Task<OperationResult<IEnumerable<SpeakerDto>>> SearchAsync(string search);
        Task<OperationResult<SpeakerDto?>> GetAsync();
    }

    class SpeakerService : ISpeakerService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _accessor;

        public SpeakerService(ApplicationDbContext dbContext, IHttpContextAccessor accessor)
        {
            _dbContext = dbContext;
            _accessor = accessor;
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
            => new(input.Name, input.LastName, new(input.Email), input.Twitter, input.Description, input.Image, Guid.Empty);


        private SpeakerDto ConvertToDto(Speaker speaker)
            => speaker.ConvertToDto();

        private IEnumerable<SpeakerDto> ConvertToDto(IEnumerable<Speaker> speakers)
            => speakers.Select(x => x.ConvertToDto());


        private async Task<Maybe<IEnumerable<Speaker>>> GetAllSpeakersAsync()
        {
            List<Speaker> speakers = new();


            var speakersMany = await _dbContext.Proposals.AsNoTracking()
               .Include(x => x.Speakers)
               .OrderByDescending(x => x.CreationTime)
               .Select(x => x.Speakers)
               .ToListAsync();

            foreach (var speakers1 in speakersMany)
            {
                foreach (var item in speakers1)
                {
                    if(!speakers.Any(x => x.Id == item.Id))
                        speakers.Add(item);
                }
            }

            return speakers;


        }


        public async Task<OperationResult<SpeakerDto?>> GetAsync()
        {
            var userId = new Guid(_accessor.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var speaker = await _dbContext.Speakers.AsNoTracking()
             .Where(x => x.UserId == userId)
             .FirstOrDefaultAsync();

            return OperationResult<SpeakerDto?>.Success(speaker is null ? null : ConvertToDto(speaker));

        }

        public async Task<OperationResult<IEnumerable<SpeakerDto>>> SearchAsync(string search)
        {

            var speaker = await _dbContext.Speakers.AsNoTracking()
             .Where(x => x.Email.Contains(search) || x.Name.Contains(search) || x.LastName.Contains(search))
             .Select(x => x.ConvertToDto())
             .ToListAsync();

            return OperationResult<IEnumerable<SpeakerDto>>.Success(speaker);

        }
    }
}
