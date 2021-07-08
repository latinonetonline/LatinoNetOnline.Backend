using CSharpFunctionalExtensions;

using LatinoNetOnline.Backend.Modules.Events.Core.Data;
using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Meetups;
using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Webinars;
using LatinoNetOnline.Backend.Modules.Events.Core.Entities;
using LatinoNetOnline.Backend.Modules.Events.Core.Events;
using LatinoNetOnline.Backend.Modules.Events.Core.Extensions;
using LatinoNetOnline.Backend.Modules.Events.Core.Validators;
using LatinoNetOnline.Backend.Shared.Abstractions.Events;
using LatinoNetOnline.Backend.Shared.Abstractions.Messaging;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Services
{
    interface IWebinarService
    {
        Task<OperationResult<WebinarDto>> CreateAsync(CreateWebinarInput input);
        Task<OperationResult<WebinarDto>> UpdateAsync(UpdateWebinarInput input);
        //Task<OperationResult<WebinarDto>> ConfirmAsync2(ConfirmWebinarInput input);
        Task<OperationResult<WebinarDto>> ConfirmAsync(ConfirmWebinarInput input);
        Task<OperationResult<WebinarDto>> ChangePhotoAsync(Guid id, Stream image);
        Task<OperationResult> DeleteAsync(Guid id);
        Task<OperationResult<WebinarDto>> GetByIdAsync(Guid id);
        Task<OperationResult<WebinarDto>> GetByProposalAsync(Guid proposalId);
        Task<OperationResult<IEnumerable<WebinarDto>>> GetAllAsync();
        Task<OperationResult<WebinarDto>> GetNextWebinarAsync();
    }

    class WebinarService : IWebinarService
    {
        private readonly EventDbContext _dbContext;
        private readonly IMeetupService _meetupService;
        private readonly IProposalService _proposalService;
        private readonly IEventDispatcher _eventDispatcher;

        public WebinarService(EventDbContext dbContext, IMeetupService meetupService, IProposalService proposalService, IEventDispatcher eventDispatcher)
        {
            _dbContext = dbContext;
            _meetupService = meetupService;
            _proposalService = proposalService;
            _eventDispatcher = eventDispatcher;
        }

        public Task<OperationResult<WebinarDto>> CreateAsync(CreateWebinarInput input)
            => Validate(input)
                .Map(ConvertToEntity)
                .Map(SetWebinarNumberAsync)
                .Map(CreateMeetupAsync)
                .Map(AddWebinarAsync)
                .Map(ConvertToDto)
                .FinallyOperationResult();


        public Task<OperationResult<WebinarDto>> UpdateAsync(UpdateWebinarInput input)
            => Validate(input)
                .Map(UpdateWebinarAsync)
                .Tap(async webinar => await _eventDispatcher.PublishAsync(new WebinarUpdatedEventInput(webinar.Id)))
                .Map(ConvertToDto)
                .FinallyOperationResult();

        public Task<OperationResult<IEnumerable<WebinarDto>>> GetAllAsync()
            => GetAllWebinars()
                .ToResult("No existen webinars.")
                .Map(ConvertToDto)
                .FinallyOperationResult();

        public Task<OperationResult<WebinarDto>> GetByIdAsync(Guid id)
            => GetWebinarById(id)
                .ToResult("No existe un webinar con ese id.")
                .Map(ConvertToDto)
                .FinallyOperationResult();

        public Task<OperationResult<WebinarDto>> GetNextWebinarAsync()
            => GetNextWebinar()
                .ToResult("No hay un siguiente webinar creado.")
                .Map(ConvertToDto)
                .FinallyOperationResult();


        public async Task<OperationResult> DeleteAsync(Guid id)
            => await GetWebinarById(id)
                .ToResult("No existe un webinar con ese id.")
                .Tap(RemoveWebinarAsync)
                .FinallyOperationResult();

        public async Task<OperationResult<WebinarDto>> GetByProposalAsync(Guid proposalId)
            => await GetWebinarByProposal(proposalId)
                .ToResult("No existe un webinar con esa propuesta.")
                .Map(ConvertToDto)
                .FinallyOperationResult();


        private Result<CreateWebinarInput> Validate(CreateWebinarInput input)
            => new CreateWebinarValidator(_proposalService, _meetupService).Validate(input).ToResult(input);

        private Result<UpdateWebinarInput> Validate(UpdateWebinarInput input)
            => new UpdateWebinarValidator(_meetupService).Validate(input).ToResult(input);

        private async Task<Result<Webinar>> Validate(ConfirmWebinarInput input)
        {
            var webinar = await GetWebinarById(input.Id);
            if (webinar.HasNoValue)
            {
                return Result.Failure<Webinar>("No existe webinar con ese Id.");
            }

            return new ConfirmWebinarValidator().Validate(webinar.Value).ToResult(webinar.Value);
        }


        private async Task<Maybe<IEnumerable<Webinar>>> GetAllWebinars()
            => await _dbContext.Webinars
                .Where(x => x.Status == Enums.WebinarStatus.Draft || x.Status == Enums.WebinarStatus.Published)
                .ToListAsync();


        private async Task<Maybe<Webinar>> GetWebinarById(Guid id)
            => await _dbContext.Webinars
                .SingleOrDefaultAsync(x => x.Id == id);

        private async Task<Maybe<Webinar>> GetWebinarByProposal(Guid proposalId)
            => await _dbContext.Webinars
                .SingleOrDefaultAsync(x => x.ProposalId == proposalId);



        private async Task<Maybe<Webinar>> GetNextWebinar()
            => await _dbContext.Webinars
                .Where(x => x.StartDateTime > DateTime.Now)
                .OrderBy(x => x.StartDateTime)
                .FirstOrDefaultAsync();



        private async Task<Webinar> AddWebinarAsync(Webinar webinar)
        {
            await _dbContext.Webinars.AddAsync(webinar);
            await _dbContext.SaveChangesAsync();

            return webinar;
        }

        private async Task<Webinar> UpdateWebinarAsync(UpdateWebinarInput input)
        {
            var webinar = await _dbContext.Webinars.SingleAsync(x => x.Id == input.Id);

            webinar.Number = input.Number;
            webinar.StartDateTime = input.StartDateTime;
            webinar.Streamyard = input.Streamyard;
            webinar.LiveStreaming = input.LiveStreaming;
            webinar.Flyer = input.Flyer;
            webinar.Status = input.Status;

            _dbContext.Update(webinar);

            await _dbContext.SaveChangesAsync();

            return webinar;
        }

        private async Task<Webinar> CreateMeetupAsync(Webinar webinar)
        {
            var proposalResult = await _proposalService.GetByIdAsync(new(webinar.ProposalId));

            var meetup = await _meetupService.CreateEventAsync(new(proposalResult.Result.Proposal.Title, webinar.ConvertToDto().GetDescription(proposalResult.Result), webinar.StartDateTime));

            if (meetup.IsSuccess && meetup.Result.Id is not null)
            {
                webinar.MeetupId = meetup.Result.NormalizeId();
                webinar.Status = Enums.WebinarStatus.Draft;
            }

            return webinar;
        }

        private async Task<Webinar> SetWebinarNumberAsync(Webinar webinar)
        {
            int? maxWebinarNumber = await _dbContext.Webinars.MaxNumberAsync();
            webinar.Number = maxWebinarNumber.GetValueOrDefault() + 1;

            return webinar;
        }

        private Webinar ConvertToEntity(CreateWebinarInput input)
            => input.ConvertToEntity();


        private WebinarDto ConvertToDto(Webinar webinar)
            => webinar.ConvertToDto();

        private IEnumerable<WebinarDto> ConvertToDto(IEnumerable<Webinar> webinars)
            => webinars.Select(ConvertToDto);


        private async Task RemoveWebinarAsync(Webinar webinar)
        {
            _dbContext.Webinars.Remove(webinar);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<Webinar> MappingFlyerLinkAsync(Webinar webinar, Stream image)
        {
            var proposalResult = await _proposalService.GetByIdAsync(new(webinar.ProposalId));

            var photoResult = await _meetupService.UploadPhotoAsync(webinar.MeetupId, image);


            webinar.Flyer = photoResult.Result.HighresLink;

            UpdateMeetupEventInput updateMeetupEventInput = new(webinar.MeetupId, proposalResult.Result.Proposal.Title, webinar.ConvertToDto().GetDescription(proposalResult.Result), proposalResult.Result.Proposal.EventDate, webinar.LiveStreaming, photoResult.Result?.Id);

            var meetupResult = await _meetupService.UpdateEventAsync(updateMeetupEventInput);

            if (meetupResult.IsSuccess)
            {
                _dbContext.Update(webinar);

                await _dbContext.SaveChangesAsync();
            }

            return webinar;
        }

        private async Task<Webinar> PublishEventAsync(Webinar webinar)
        {
            var result = await _meetupService.PublishEventAsync(webinar.MeetupId);

            if (result.IsSuccess)
                result = await _meetupService.AnnounceEventAsync(webinar.MeetupId);

            if (result.IsSuccess)
            {
                webinar.Status = Enums.WebinarStatus.Published;

                _dbContext.Update(webinar);

                await _dbContext.SaveChangesAsync();
            }


            return webinar;
        }

        public Task<OperationResult<WebinarDto>> ConfirmAsync(ConfirmWebinarInput input)
            => Validate(input)
                .Map(PublishEventAsync)
                .Map(ConvertToDto)
                .FinallyOperationResult();

        public Task<OperationResult<WebinarDto>> ChangePhotoAsync(Guid id, Stream image)
             => GetWebinarById(id)
                    .ToResult("No existe un webinar con ese id.")
                    .Map(webinar => MappingFlyerLinkAsync(webinar, image))
                    .Map(ConvertToDto)
                    .FinallyOperationResult();
    }
}
