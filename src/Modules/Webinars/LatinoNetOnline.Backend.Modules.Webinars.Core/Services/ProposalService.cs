using CSharpFunctionalExtensions;

using LatinoNetOnline.Backend.Modules.Webinars.Core.Data;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Speakers;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Events;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Extensions;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Managers;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Validators;
using LatinoNetOnline.Backend.Shared.Abstractions.Messaging;
using LatinoNetOnline.Backend.Shared.Commons.Extensions;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Services
{
    interface IProposalService
    {
        Task<OperationResult<ProposalFullDto>> CreateAsync(CreateProposalInput input);
        Task<OperationResult<ProposalFullDto>> UpdateAsync(UpdateProposalInput input);
        Task<OperationResult> DeleteAsync(Guid id);
        Task<OperationResult> DeleteAllAsync();
        Task<OperationResult<IEnumerable<ProposalFullDto>>> GetAllAsync(ProposalFilter filter);
        Task<OperationResult<ProposalDateDto>> GetDatesAsync();
        Task<OperationResult<ProposalFullDto>> GetByIdAsync(GetProposalInput input);
        Task<OperationResult<ProposalDto>> ChangePhotoAsync(Guid id, byte[] image);
        Task<OperationResult<ProposalDto>> ConfirmProposalAsync(ConfirmProposalInput input);
        Task<OperationResult> UpdateWebinarNumbersAsync();
        Task<OperationResult<ProposalDescriptionText>> GetDescriptionTextAsync(GetProposalDescriptionTextInput input);
        Task<OperationResult> UpdateViewsAsync(UpdateProposalViewsInput input);
    }

    class ProposalService : IProposalService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IEmailManager _emailManager;
        private readonly IMessageBroker _messageBroker;
        private readonly IStorageService _storageService;
        private readonly IHttpContextAccessor _accessor;

        public ProposalService(ApplicationDbContext dbContext, IEmailManager emailManager, IMessageBroker messageBroker, IStorageService storageService, IHttpContextAccessor accessor)
        {
            _dbContext = dbContext;
            _emailManager = emailManager;
            _messageBroker = messageBroker;
            _storageService = storageService;
            _accessor = accessor;
        }

        public Task<OperationResult<IEnumerable<ProposalFullDto>>> GetAllAsync(ProposalFilter filter)
            => GetProposals(filter, true)
                .ToResult("No hay ninguna propuesta.")
                .Map(ConvertToFullDto)
                .FinallyOperationResult();

        public Task<OperationResult<ProposalDateDto>> GetDatesAsync()
            => GetProposalDates()
                .ToResult("No hay ninguna propuesta.")
                .Map(dates => new ProposalDateDto(dates))
                .FinallyOperationResult();


        public Task<OperationResult<ProposalFullDto>> GetByIdAsync(GetProposalInput input)
            => GetProposalById(input.Id, true)
                .ToResult("No existe una propuesta con ese id.")
                .Map(ConvertToFullDto)
                .FinallyOperationResult();


        public async Task<OperationResult> DeleteAsync(Guid id)
            => await GetProposalById(id, false)
                .ToResult("No existe una propuesta con ese id.")
                .Tap(RemoveProposalAsync)
                .Tap(async () => await _messageBroker.PublishAsync(new ProposalDeletedEventInput(id)))
                .FinallyOperationResult();


        public Task<OperationResult<ProposalFullDto>> CreateAsync(CreateProposalInput input)
            => Validate(input)
                    .Map(CreateProposalAsync)
                    //.Tap(async proposal => await _messageBroker.PublishAsync(new ProposalCreatedEventInput(proposal.Proposal.ProposalId, proposal.Proposal.Title)))
                    .Check(async proposal => await _emailManager.SendEmailAsync(await proposal.ConvertToProposalCreatedEmailInput()))
                    .FinallyOperationResult();

        public Task<OperationResult<ProposalFullDto>> UpdateAsync(UpdateProposalInput input)
            => Validate(input)
                    .Map(UpdateProposalAsync)
                    .Tap(async proposal => await _messageBroker.PublishAsync(new ProposalUpdatedEventInput(proposal.Proposal.ProposalId)))
                    .FinallyOperationResult();


        public async Task<OperationResult> DeleteAllAsync()
            => await GetProposals(new(), false)
                        .ToResult("No hay ninguna propuesta.")
                        .Tap(RemoveProposalAsync)
                        .FinallyOperationResult();


        private async Task<Maybe<List<Proposal>>> GetProposals(ProposalFilter filter, bool include)
            => await _dbContext.Proposals.AsNoTracking()

                    .WhereIf(!string.IsNullOrWhiteSpace(filter.Title), x => x.Title.Contains((filter.Title ?? string.Empty).ToLower()))
                    .WhereIf(filter.Date.HasValue, x => x.EventDate.Date == filter.Date.GetValueOrDefault())
                    .WhereIf(filter.IsActive.HasValue, x => x.IsActive == filter.IsActive.GetValueOrDefault())
                    .WhereIf(filter.Oldest.HasValue && filter.Oldest.Value, x => x.EventDate.Date < DateTime.Today)
                    .WhereIf(!filter.Oldest.HasValue || !filter.Oldest.Value, x => x.EventDate.Date >= DateTime.Today)
                    .IncludeIf(include, x => x.Speakers)
                    .OrderByDescending(x => x.EventDate).ToListAsync();


        private async Task<Maybe<List<DateTime>>> GetProposalDates()
            => await _dbContext.Proposals.AsNoTracking()
            .Where(x => x.EventDate >= DateTime.Today)
            .Select(x => x.EventDate)
            .Union(_dbContext.UnavailableDates.AsNoTracking()
                .Where(x => x.Date >= DateTime.Today)
                .Select(x => x.Date))
            .ToListAsync();


        private async Task<Maybe<Proposal>> GetProposalById(Guid id, bool include)
            => await _dbContext.Proposals.IncludeIf(include, x => x.Speakers).SingleOrDefaultAsync(x => x.Id == id);


        private Result<CreateProposalInput> Validate(CreateProposalInput input)
        {
            CreateProposalValidator validator = new(_dbContext);

            var validationResult = validator.Validate(input);

            return validationResult.ToResult(input);

        }

        private Result<UpdateProposalInput> Validate(UpdateProposalInput input)
        {
            UpdateProposalValidator validator = new(_dbContext);

            var validationResult = validator.Validate(input);

            return validationResult.ToResult(input);

        }

        private async Task<ProposalFullDto> CreateProposalAsync(CreateProposalInput input)
        {
            Proposal proposal = new(input.Title, input.Description, input.AudienceAnswer, input.KnowledgeAnswer, input.UseCaseAnswer, input.Date);

            List<SpeakerDto> speakerdtos = new();

            var userId = new Guid(_accessor.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var userEmail = _accessor.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email).Value;


            var speakers = await _dbContext.Speakers.Where(x => x.UserId == userId || input.Speakers.Select(s => s.Email).Contains(x.Email)).ToListAsync();

            foreach (var speakerInput in input.Speakers)
            {
                Guid newUserId = userEmail == speakerInput.Email ? userId : Guid.Empty;

                if (speakers.Select(x => x.Email).Distinct().Contains(speakerInput.Email))
                {
                    Speaker speaker = speakers.First(x => x.UserId == userId || x.Email == speakerInput.Email);
                    speaker.Name = speakerInput.Name;
                    speaker.LastName = speakerInput.LastName;
                    speaker.LastName = speakerInput.LastName;
                    speaker.Twitter = speakerInput.Twitter;
                    speaker.Description = speakerInput.Description;
                    speaker.Image = speakerInput.Image;

                    if (speaker.UserId == Guid.Empty)
                        speaker.UserId = newUserId;

                    proposal.Speakers.Add(speaker);

                    speakerdtos.Add(speaker.ConvertToDto());
                }
                else
                {
                    Speaker speaker = new(speakerInput.Name, speakerInput.LastName, speakerInput.Email, speakerInput.Twitter, speakerInput.Description, speakerInput.Image, newUserId);

                    proposal.Speakers.Add(speaker);

                    speakerdtos.Add(speaker.ConvertToDto());
                }

            }

            await SetWebinarNumberAsync(proposal);

            await _dbContext.AddAsync(proposal);

            await _dbContext.SaveChangesAsync();

            ProposalFullDto proposalFullDto = new(proposal.ConvertToDto(), speakerdtos); ;

            return proposalFullDto;
        }

        private async Task<ProposalFullDto> UpdateProposalAsync(UpdateProposalInput input)
        {
            Proposal proposal = await _dbContext.Proposals.Include(x => x.Speakers).SingleAsync(x => x.Id == input.Id);

            proposal.Title = input.Title;
            proposal.Description = input.Description;
            proposal.AudienceAnswer = input.AudienceAnswer;
            proposal.KnowledgeAnswer = input.KnowledgeAnswer;
            proposal.UseCaseAnswer = input.UseCaseAnswer;
            proposal.WebinarNumber = input.WebinarNumber;
            proposal.Meetup = input.Meetup;
            proposal.Streamyard = input.Streamyard;
            proposal.LiveStreaming = input.LiveStreaming;
            proposal.Flyer = input.Flyer;
            proposal.EventDate = input.Date;
            proposal.Views = input.Views;
            proposal.LiveAttends = input.LiveAttends;

            proposal.Speakers = new List<Speaker>();

            List<SpeakerDto> speakerdtos = new();

            foreach (var speakerInput in input.Speakers)
            {

                Speaker speaker = await _dbContext.Speakers.SingleOrDefaultAsync(x => x.Id == speakerInput.Id);

                if (speaker is not null)
                {
                    speaker.Name = speakerInput.Name;
                    speaker.LastName = speakerInput.LastName;
                    speaker.Email = new(speakerInput.Email);
                    speaker.Twitter = speakerInput.Twitter;
                    speaker.Description = speakerInput.Description;
                    speaker.Image = speakerInput.Image;

                    proposal.Speakers.Add(speaker);

                    speakerdtos.Add(speaker.ConvertToDto());

                }
            }

            _dbContext.Update(proposal);

            await _dbContext.SaveChangesAsync();

            ProposalFullDto proposalFullDto = new(proposal.ConvertToDto(), speakerdtos); ;

            return proposalFullDto;
        }

        private async Task RemoveProposalAsync(Proposal proposal)
        {
            if (proposal.Speakers is not null)
            {
                foreach (Speaker speaker in proposal.Speakers)
                {
                    _dbContext.Speakers.Remove(speaker);
                }
            }

            _dbContext.Proposals.Remove(proposal);
            await _dbContext.SaveChangesAsync();
        }

        private async Task RemoveProposalAsync(IEnumerable<Proposal> proposals)
        {

            foreach (var item in proposals)
                await RemoveProposalAsync(item);

        }

        private ProposalFullDto ConvertToFullDto(Proposal proposal)
        {
            var proposalDto = proposal.ConvertToDto();
            var speakers = proposal.Speakers;
            var speakerDtos = speakers.Select(x => x.ConvertToDto());

            return new(proposalDto, speakerDtos);
        }

        private IEnumerable<ProposalFullDto> ConvertToFullDto(IEnumerable<Proposal> proposals)
        {
            List<ProposalFullDto> proposalFullDtos = new();

            foreach (var proposal in proposals)
            {
                var proposalFullDto = ConvertToFullDto(proposal);
                proposalFullDtos.Add(proposalFullDto);
            }

            return proposalFullDtos;
        }

        public async Task<OperationResult<ProposalDto>> ChangePhotoAsync(Guid id, byte[] image)
        {
            var proposal = await _dbContext.Proposals.SingleOrDefaultAsync(x => x.Id == id);


            var imageResult = await _storageService.UploadFile("flyers", Guid.NewGuid().ToString() + ".jpg", image);


            if (imageResult.IsSuccess)
            {
                proposal.Flyer = imageResult.Result;

                _dbContext.Update(proposal);

                await _dbContext.SaveChangesAsync();
            }

            return OperationResult<ProposalDto>.Success(proposal.ConvertToDto());
        }


        public async Task<OperationResult<ProposalDto>> ConfirmProposalAsync(ConfirmProposalInput input)
        {
            var proposal = await _dbContext.Proposals.Include(x => x.Speakers).SingleOrDefaultAsync(x => x.Id == input.Id);

            if (proposal is not null)
            {
                proposal.Status = Enums.WebinarStatus.Published;

                _dbContext.Update(proposal);

                await _dbContext.SaveChangesAsync();


                ProposalFullDto proposalFullDto = new(proposal.ConvertToDto(), proposal.Speakers.Select(x => x.ConvertToDto()));


                var emailInput = await proposalFullDto.ConvertToProposalConfirmedEmailInput();

                var emailResult = await _emailManager.SendEmailAsync(emailInput);

                if (!emailResult.IsSuccess)
                {
                    return OperationResult<ProposalDto>.Fail(new("Hubo un error al enviar el Email."));

                }

                return OperationResult<ProposalDto>.Success(proposal.ConvertToDto());
            }

            return OperationResult<ProposalDto>.Fail(new("No existe la propuesta"));


        }

        private async Task<Proposal> SetWebinarNumberAsync(Proposal webinar)
        {
            int? maxWebinarNumber = await _dbContext.Proposals.MaxNumberAsync();
            webinar.WebinarNumber = maxWebinarNumber.GetValueOrDefault() + 1;

            return webinar;
        }


        public async Task<OperationResult> UpdateWebinarNumbersAsync()
        {
            var webinars = await _dbContext.Proposals
                .Where(x => x.EventDate.Date >= DateTime.Today)
                .ToListAsync();


            if (!webinars.Any()) return OperationResult.Fail(new("No hay propuestas"));


            var lastWebinarNumberConfirmated = await _dbContext.Proposals
                .Where(x => x.Status == Enums.WebinarStatus.Published && x.EventDate.Date < DateTime.Today)
                .OrderByDescending(x => x.EventDate)
                .Select(x => x.WebinarNumber)
                .FirstOrDefaultAsync();


            if (!lastWebinarNumberConfirmated.HasValue) return OperationResult.Fail(new("No hay un último numero de webinar publicado."));


            webinars.UpdateWebinarNumber(lastWebinarNumberConfirmated.Value);

            _dbContext.Proposals.UpdateRange(webinars);

            await _dbContext.SaveChangesAsync();


            //foreach (var item in webinars)
            //{
            //    await _eventDispatcher.PublishAsync(new WebinarUpdatedEventInput(item.Id));
            //}

            return OperationResult.Success();
        }

        public async Task<OperationResult<ProposalDescriptionText>> GetDescriptionTextAsync(GetProposalDescriptionTextInput input)
        {
            var proposal = await _dbContext.Proposals.AsNoTracking().Include(x => x.Speakers).SingleOrDefaultAsync(x => x.Id == input.Id);

            ProposalFullDto proposalFullDto = new(proposal.ConvertToDto(), proposal.Speakers.Select(x => x.ConvertToDto()));

            return OperationResult<ProposalDescriptionText>.Success(new(proposalFullDto.GetDescription()));

        }

        public async Task<OperationResult> UpdateViewsAsync(UpdateProposalViewsInput input)
        {
            var proposals = await _dbContext.Proposals.Where(x => input.Views.Select(v => v.Id).Contains(x.Id)).ToListAsync();

            foreach (var item in proposals)
            {
                var views = input.Views.FirstOrDefault(x => x.Id == item.Id);

                if(views is not null && views.Views.HasValue)
                {
                    item.Views = views.Views.Value;
                }
            }

            _dbContext.Proposals.UpdateRange(proposals);

            await _dbContext.SaveChangesAsync();

            return OperationResult.Success();
        }
    }
}
