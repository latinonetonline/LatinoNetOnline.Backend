using CSharpFunctionalExtensions;

using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Data;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Webinars;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Entities;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Extensions;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Validators;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Services
{
    interface IWebinarService
    {
        Task<OperationResult<WebinarDto>> CreateAsync(CreateWebinarInput input);
        Task<OperationResult> DeleteAsync(Guid id);
        Task<OperationResult<WebinarFullDto>> GetByIdAsync(Guid id);
        Task<OperationResult<WebinarFullDto>> GetByProposalAsync(Guid proposalId);
        Task<OperationResult<IEnumerable<WebinarFullDto>>> GetAllAsync();
        Task<OperationResult<WebinarFullDto>> GetNextWebinarAsync();
    }

    class WebinarService : IWebinarService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMeetupService _meetupService;

        public WebinarService(ApplicationDbContext dbContext, IMeetupService meetupService)
        {
            _dbContext = dbContext;
            _meetupService = meetupService;
        }

        public Task<OperationResult<WebinarDto>> CreateAsync(CreateWebinarInput input)
            => Validate(input)
                .Map(ConvertToEntity)
                .Map(AddWebinarAsync)
                .Map(ConvertToDto)
                .FinallyOperationResult();

        public Task<OperationResult<IEnumerable<WebinarFullDto>>> GetAllAsync()
            => GetAllWebinars()
                .ToResult("No existen webinars.")
                .Map(ConvertToFullDto)
                .FinallyOperationResult();

        public Task<OperationResult<WebinarFullDto>> GetByIdAsync(Guid id)
            => GetWebinarById(id)
                .ToResult("No existe un webinar con ese id.")
                .Map(ConvertToFullDto)
                .FinallyOperationResult();

        public Task<OperationResult<WebinarFullDto>> GetNextWebinarAsync()
            => GetNextWebinar()
                .ToResult("No hay un siguiente webinar creado.")
                .Map(ConvertToFullDto)
                .FinallyOperationResult();


        public async Task<OperationResult> DeleteAsync(Guid id)
            => await GetWebinarById(id)
                .ToResult("No existe un webinar con ese id.")
                .Tap(RemoveWebinarAsync)
                .FinallyOperationResult();

        public async Task<OperationResult<WebinarFullDto>> GetByProposalAsync(Guid proposalId)
            => await GetWebinarByProposal(proposalId)
                .ToResult("No existe un webinar con esa propuesta.")
                .Map(ConvertToFullDto)
                .FinallyOperationResult();


        private Result<CreateWebinarInput> Validate(CreateWebinarInput input)
        => new CreateWebinarValidator(_dbContext, _meetupService).Validate(input).ToResult(input);

        private async Task<Maybe<IEnumerable<Webinar>>> GetAllWebinars()
            => await _dbContext.Webinars
                .Include(x => x.Proposal)
                .ThenInclude(x => x!.Speakers)
                .ToListAsync();


        private async Task<Maybe<Webinar>> GetWebinarById(Guid id)
            => await _dbContext.Webinars
                .Include(x => x.Proposal)
                .ThenInclude(x => x!.Speakers)
                .SingleOrDefaultAsync(x => x.Id == id);

        private async Task<Maybe<Webinar>> GetWebinarByProposal(Guid proposalId)
            => await _dbContext.Webinars
                .Include(x => x.Proposal)
                .ThenInclude(x => x!.Speakers)
                .SingleOrDefaultAsync(x => x.ProposalId == proposalId);



        private async Task<Maybe<Webinar>> GetNextWebinar()
            => await _dbContext.Webinars
                .Include(x => x.Proposal)
                .ThenInclude(x => x!.Speakers)
                .Where(x => x.Proposal!.IsActive && x.Proposal.EventDate > DateTime.Now)
                .OrderBy(x => x.Proposal!.EventDate)
                .FirstOrDefaultAsync();

        

        private async Task<Webinar> AddWebinarAsync(Webinar webinar)
        {
            await _dbContext.Webinars.AddAsync(webinar);
            await _dbContext.SaveChangesAsync();

            return webinar;
        }

        private async Task<Webinar> ConvertToEntity(CreateWebinarInput input)
        {
            var webinar = input.ConvertToEntity();

            var meetup = await _meetupService.GetMeetupAsync(input.MeetupId);

            webinar.LiveStreaming = meetup.Result.How_To_Find_Us;
            webinar.Flyer = new(meetup.Result.Featured_Photo.Highres_Link);

            return webinar;
        }

        private WebinarDto ConvertToDto(Webinar webinar)
            => webinar.ConvertToDto();


        private WebinarFullDto ConvertToFullDto(Webinar webinar)
            => new(webinar.ConvertToDto(),
                 webinar.Proposal!.ConvertToDto(),
                 webinar.Proposal!.Speakers.Select(x => x.ConvertToDto()));

        private IEnumerable<WebinarFullDto> ConvertToFullDto(IEnumerable<Webinar> webinars)
            => webinars.Select(ConvertToFullDto);


        private async Task RemoveWebinarAsync(Webinar webinar)
        {
            _dbContext.Webinars.Remove(webinar);
            await _dbContext.SaveChangesAsync();
        }
    }
}
