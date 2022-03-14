
using LatinoNetOnline.Backend.Modules.Events.Core.Data;
using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Meetups;
using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Webinars;
using LatinoNetOnline.Backend.Modules.Events.Core.Entities;
using LatinoNetOnline.Backend.Modules.Events.Core.Enums;
using LatinoNetOnline.Backend.Modules.Events.Core.Extensions;
using LatinoNetOnline.Backend.Modules.Events.Core.Validators;
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
        Task<OperationResult<WebinarDto>> UpdateAsync(UpdateWebinarInput input);
        Task<OperationResult<WebinarDto>> ConfirmAsync(ConfirmWebinarInput input);
        Task<OperationResult<WebinarDto>> ChangePhotoAsync(Guid id, Stream image);
        Task<OperationResult<WebinarDto>> GetByIdAsync(GetWebinarInput input);
        Task<OperationResult<WebinarDto>> GetByProposalAsync(Guid proposalId);
        Task<OperationResult<IEnumerable<WebinarDto>>> GetAllAsync();
        Task<OperationResult<WebinarDto>> GetNextWebinarAsync();
        Task<OperationResult> UpdateWebinarNumbersAsync();
    }

    class WebinarService : IWebinarService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMeetupService _meetupService;
        private readonly IProposalService _proposalService;
        private readonly IMessageBroker _messageBroker;

        public WebinarService(ApplicationDbContext dbContext, IMeetupService meetupService, IProposalService proposalService, IMessageBroker messageBroker)
        {
            _dbContext = dbContext;
            _meetupService = meetupService;
            _proposalService = proposalService;
            _messageBroker = messageBroker;
        }

        public async Task<OperationResult<WebinarDto>> UpdateAsync(UpdateWebinarInput input)
        {
            var validator = await new UpdateWebinarValidator(_meetupService).ValidateAsync(input);

            if (validator.IsValid)
            {
                var webinar = await _dbContext.Webinars.SingleAsync(x => x.Id == input.Id);

                webinar.Streamyard = input.Streamyard;
                webinar.LiveStreaming = input.LiveStreaming;
                webinar.Flyer = input.Flyer;
                webinar.Status = input.Status;

                _dbContext.Update(webinar);

                await _dbContext.SaveChangesAsync();

                var dto = webinar.ConvertToDto();

                return new(dto);
            }

            return validator.ToOperationResult<WebinarDto>();
        }

        public async Task<OperationResult<IEnumerable<WebinarDto>>> GetAllAsync()
        {
            var webinars = await _dbContext.Webinars
                .Include(w => w.Proposal)
                .Where(x => x.Status == WebinarStatus.Draft || x.Status == WebinarStatus.Published)
                .Where(x => x.Proposal.EventDate.Date >= DateTime.Today)
                .OrderByDescending(x => x.Number)
                .ToListAsync();

            return new(webinars.Select(x => x.ConvertToDto()));
        }


        public async Task<OperationResult<WebinarDto>> GetByIdAsync(GetWebinarInput input)
        {
            var webinar = await GetWebinarById(input.Id);
            if (webinar is not null)
                return new(webinar.ConvertToDto());
            else
                return OperationResult<WebinarDto>.Fail("No se encontro el webinar");
        }



        public async Task<OperationResult<WebinarDto>> GetNextWebinarAsync()
        {
            var webinar = await _dbContext.Webinars
                .Include(w => w.Proposal)
                .Where(x => x.Proposal.EventDate.Date > DateTime.Now)
                .OrderBy(x => x.Proposal.EventDate)
                .FirstOrDefaultAsync();

            if (webinar is not null)
                return new(webinar.ConvertToDto());
            else
                return OperationResult<WebinarDto>.Fail("No se encontro el webinar");
        }

        public async Task<OperationResult<WebinarDto>> GetByProposalAsync(Guid proposalId)
        {
            var webinar = await _dbContext.Webinars
                .SingleOrDefaultAsync(x => x.ProposalId == proposalId);

            return new(webinar.ConvertToDto());
        }


        private async Task<Webinar> GetWebinarById(Guid id)
            => await _dbContext.Webinars
                .SingleOrDefaultAsync(x => x.Id == id);

        private async Task<Webinar> MappingFlyerLinkAsync(Webinar webinar, Stream image)
        {
            var proposalResult = await _proposalService.GetByIdAsync(new(webinar.ProposalId));

            UploadImageInput uploadImageInput = new("EVENT_PHOTO", webinar.Id + ".jpeg", "JPEG", image);

            var photoResult = await _meetupService.UploadPhotoAsync(uploadImageInput);


            webinar.Flyer = new Uri("https://secure.meetupstatic.com" + photoResult.Result.ImagePath);

            UpdateMeetupEventInput updateMeetupEventInput = new(webinar.MeetupId, proposalResult.Result.Proposal.Title, webinar.ConvertToDto().GetDescription(proposalResult.Result), proposalResult.Result.Proposal.EventDate.Date, webinar.LiveStreaming, long.Parse(photoResult.Result.Image.Id));

            var meetupResult = await _meetupService.UpdateEventAsync(updateMeetupEventInput);

            if (meetupResult.IsSuccess)
            {
                _dbContext.Update(webinar);

                await _dbContext.SaveChangesAsync();
            }

            return webinar;
        }


        public async Task<OperationResult<WebinarDto>> ConfirmAsync(ConfirmWebinarInput input)
        {

            var webinar = await GetWebinarById(input.Id);
            if (webinar is not null)
            {
                var validator = await new ConfirmWebinarValidator().ValidateAsync(webinar);

                if (validator.IsValid)
                {

                    //TODO: Service Bus
                    var result = await _meetupService.PublishEventAsync(webinar.MeetupId);

                    if (result.IsSuccess)
                    {
                        result = await _meetupService.AnnounceEventAsync(webinar.MeetupId);
                    }

                    if (result.IsSuccess)
                    {
                        webinar.Status = WebinarStatus.Published;

                        _dbContext.Update(webinar);

                        await _dbContext.SaveChangesAsync();
                    }


                    return new(webinar.ConvertToDto());
                }

                return validator.ToOperationResult<WebinarDto>();

            }

            return OperationResult<WebinarDto>.Fail("No existe webinar con ese Id.");

        }


        public async Task<OperationResult<WebinarDto>> ChangePhotoAsync(Guid id, Stream image)
        {
            var webinar = await GetWebinarById(id);

            if (webinar is not null)
            {
                webinar = await MappingFlyerLinkAsync(webinar, image);

                return new(webinar.ConvertToDto());
            }

            return OperationResult<WebinarDto>.Fail("No existe un webinar con ese id.");
        }

        public async Task<OperationResult> UpdateWebinarNumbersAsync()
        {
            var webinars = await _dbContext.Webinars
                .Include(webinar => webinar.Proposal)
                .Where(x => x.Status == WebinarStatus.Draft || x.Status == WebinarStatus.Published)
                .Where(x => x.Proposal.EventDate.Date >= DateTime.Today)
                .ToListAsync();


            if (!webinars.Any())
            {
                return OperationResult.Fail(new("No hay webinars"));
            }

            var lastWebinarNumberConfirmated = await _dbContext.Webinars
                .Include(webinar => webinar.Proposal)
                .Where(x => x.Status == WebinarStatus.Published && x.Proposal.EventDate.Date < DateTime.Today)
                .OrderByDescending(x => x.Proposal.EventDate.Date)
                .Select(x => x.Number)
                .FirstOrDefaultAsync();


            webinars.UpdateWebinarNumber(lastWebinarNumberConfirmated);

            _dbContext.Webinars.UpdateRange(webinars);

            await _dbContext.SaveChangesAsync();


            foreach (var item in webinars)
            {

                //TODO: Service Bus
                //await _eventDispatcher.PublishAsync(new WebinarUpdatedEventInput(item.Id));
            }

            return OperationResult.Success();
        }
    }
}
