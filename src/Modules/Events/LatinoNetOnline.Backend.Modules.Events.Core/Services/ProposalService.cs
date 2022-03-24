
using LatinoNetOnline.Backend.Modules.Events.Core.Data;
using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Speakers;
using LatinoNetOnline.Backend.Modules.Events.Core.Entities;
using LatinoNetOnline.Backend.Modules.Events.Core.Enums;
using LatinoNetOnline.Backend.Modules.Events.Core.Extensions;
using LatinoNetOnline.Backend.Modules.Events.Core.Managers;
using LatinoNetOnline.Backend.Modules.Events.Core.Validators;
using LatinoNetOnline.Backend.Shared.Abstractions.Messaging;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Services
{
    interface IProposalService
    {
        Task<OperationResult<ProposalFullDto>> CreateAsync(CreateProposalInput input);
        Task<OperationResult<ProposalFullDto>> UpdateAsync(UpdateProposalInput input);
        Task<OperationResult> DeleteAsync(Guid id);
        Task<OperationResult<IEnumerable<ProposalFullDto>>> GetAllAsync(ProposalFilter filter);
        Task<OperationResult<ProposalDateDto>> GetDatesAsync();
        Task<OperationResult<ProposalFullDto>> GetByIdAsync(GetProposalInput input);
    }

    class ProposalService : IProposalService
    {
        private readonly ApplicationDbContext _dbContext;

        public ProposalService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IEnumerable<ProposalFullDto>>> GetAllAsync(ProposalFilter filter)
        {
            var proposals = await _dbContext.Proposals.AsNoTracking()
                    .Include(x => x.Speakers)
                    .WhereIf(!string.IsNullOrWhiteSpace(filter.Title), x => x.Title.Contains((filter.Title ?? string.Empty).ToLower()))
                    .WhereIf(filter.Date.HasValue, x => x.EventDate.Date == filter.Date.GetValueOrDefault())
                    .WhereIf(filter.IsActive.HasValue, x => x.IsActive == filter.IsActive.GetValueOrDefault())
                    .WhereIf(filter.Oldest.HasValue && filter.Oldest.Value, x => x.EventDate.Date < DateTime.Today)
                    .WhereIf(!filter.Oldest.HasValue || !filter.Oldest.Value, x => x.EventDate.Date >= DateTime.Today)

                    .OrderByDescending(x => x.EventDate).ToListAsync();

            return new(proposals.Select(x => new ProposalFullDto(x.ConvertToDto(), x.Speakers.Select(x => x.ConvertToDto()))));
        }

        public async Task<OperationResult<ProposalDateDto>> GetDatesAsync()
        {
            var dates = await _dbContext.Proposals.AsNoTracking()
               .Where(x => x.EventDate >= DateTime.Today)
               .Select(x => x.EventDate)
               .Union(_dbContext.UnavailableDates.AsNoTracking()
                   .Where(x => x.Date >= DateTime.Today)
                   .Select(x => x.Date))
               .ToListAsync();

            return new(new(dates));
        }

        public async Task<OperationResult<ProposalFullDto>> GetByIdAsync(GetProposalInput input)
        {
            var proposal = await GetProposalById(input.Id);

            if (proposal is not null)
                return new(new(proposal.ConvertToDto(), proposal.Speakers.Select(x => x.ConvertToDto())));
            else
                return OperationResult<ProposalFullDto>.Fail("No existe la propuesta.");
        }


        public async Task<OperationResult> DeleteAsync(Guid id)
        {
            var proposal = await GetProposalById(id);

            if (proposal is not null)
            {
                if (proposal.Speakers is not null)
                {
                    foreach (Speaker speaker in proposal.Speakers)
                    {
                        _dbContext.Speakers.Remove(speaker);
                    }
                }

                if (proposal.Webinars is not null)
                {
                    foreach (Webinar speaker in proposal.Webinars)
                    {
                        _dbContext.Webinars.Remove(speaker);
                    }
                }

                _dbContext.Proposals.Remove(proposal);
                await _dbContext.SaveChangesAsync();

                //TODO: Service Bus

                return OperationResult.Success();
            }

            return OperationResult.Fail("No existe la propuesta.");
        }



        public async Task<OperationResult<ProposalFullDto>> CreateAsync(CreateProposalInput input)
        {
            var validator = await new CreateProposalValidator(_dbContext).ValidateAsync(input);

            if (validator.IsValid)
            {
                Proposal proposal = new(input.Title, input.Description, input.AudienceAnswer, input.KnowledgeAnswer, input.UseCaseAnswer, input.Date);

                List<SpeakerDto> speakerdtos = new();

                foreach (var speakerInput in input.Speakers)
                {
                    Speaker speaker = new(speakerInput.Name, speakerInput.LastName, new(speakerInput.Email), speakerInput.Twitter, speakerInput.Description, speakerInput.Image);

                    proposal.Speakers.Add(speaker);

                    speakerdtos.Add(speaker.ConvertToDto());
                }

                int? maxWebinarNumber = await _dbContext.Webinars.MaxNumberAsync();
                var webinarNumber = maxWebinarNumber.GetValueOrDefault() + 1;

                await _dbContext.Proposals.AddAsync(proposal);

                Webinar webinar = new(proposal.Id, webinarNumber, 0, null, null, null);

                await _dbContext.Webinars.AddAsync(webinar);

                await _dbContext.SaveChangesAsync();

                await UpdateWebinarNumbersAsync();

                //TODO: Enviar mensaje al Service BUS

                ProposalFullDto proposalFullDto = new(proposal.ConvertToDto(), speakerdtos);

                return OperationResult<ProposalFullDto>.Success(proposalFullDto);
            }

            return validator.ToOperationResult<ProposalFullDto>();
        }

        public async Task<OperationResult<ProposalFullDto>> UpdateAsync(UpdateProposalInput input)
        {
            var validator = await new UpdateProposalValidator(_dbContext).ValidateAsync(input);

            if (validator.IsValid)
            {
                Proposal proposal = await _dbContext.Proposals.Include(x => x.Speakers).SingleAsync(x => x.Id == input.Id);

                proposal.Title = input.Title;
                proposal.Description = input.Description;
                proposal.AudienceAnswer = input.AudienceAnswer;
                proposal.KnowledgeAnswer = input.KnowledgeAnswer;
                proposal.UseCaseAnswer = input.UseCaseAnswer;
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

                //TODO: Service BUS

                ProposalFullDto proposalFullDto = new(proposal.ConvertToDto(), speakerdtos); ;

                return new(proposalFullDto);
            }

            return validator.ToOperationResult<ProposalFullDto>();
        }

        private Task<Proposal> GetProposalById(Guid id)
            => _dbContext.Proposals.Include(x => x.Speakers).Include(x => x.Webinars).SingleOrDefaultAsync(x => x.Id == id);


        public async Task UpdateWebinarNumbersAsync()
        {
            var webinars = await _dbContext.Webinars
                .Include(webinar => webinar.Proposal)
                .Where(x => x.Status == WebinarStatus.Draft || x.Status == WebinarStatus.Published)
                .Where(x => x.Proposal.EventDate.Date >= DateTime.Today)
                .ToListAsync();


            if (webinars.Any())
            {
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

                    //TODO: dd
                    //await _eventDispatcher.PublishAsync(new WebinarUpdatedEventInput(item.Id));
                }

            }
        }
    }
}
