using CSharpFunctionalExtensions;

using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Data;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Speakers;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Entities;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Events;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Extensions;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Managers;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Validators;
using LatinoNetOnline.Backend.Shared.Abstractions.Messaging;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Services
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
    }

    class ProposalService : IProposalService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IEmailManager _emailManager;
        private readonly IMessageBroker _messageBroker;

        public ProposalService(ApplicationDbContext dbContext, IEmailManager emailManager, IMessageBroker messageBroker)
        {
            _dbContext = dbContext;
            _emailManager = emailManager;
            _messageBroker = messageBroker;
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
                    .Tap(async proposal => await _messageBroker.PublishAsync(new ProposalCreatedEventInput(proposal.Proposal.ProposalId, proposal.Proposal.Title)))
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

            foreach (var speakerInput in input.Speakers)
            {
                Speaker speaker = new(speakerInput.Name, speakerInput.LastName, new(speakerInput.Email), speakerInput.Twitter, speakerInput.Description, speakerInput.Image);

                proposal.Speakers.Add(speaker);

                speakerdtos.Add(speaker.ConvertToDto());
            }

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
    }
}
