using CSharpFunctionalExtensions;

using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Data;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Speakers;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Entities;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Extensions;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Managers;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Validators;
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
        Task<OperationResult> DeleteAsync(Guid id);
        Task<OperationResult> DeleteAllAsync();
        Task<OperationResult<IEnumerable<ProposalFullDto>>> GetAllAsync(ProposalFilter filter);
        Task<OperationResult<ProposalDateDto>> GetAllDatesAsync();
        Task<OperationResult<ProposalFullDto>> GetByIdAsync(Guid id);
    }

    class ProposalService : IProposalService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IEmailManager _emailManager;

        public ProposalService(ApplicationDbContext dbContext, IEmailManager emailManager)
        {
            _dbContext = dbContext;
            _emailManager = emailManager;
        }

        public Task<OperationResult<IEnumerable<ProposalFullDto>>> GetAllAsync(ProposalFilter filter)
            => GetProposals(filter, true)
                .ToResult("No hay ninguna propuesta.")
                .Map(ConvertToFullDto)
                .FinallyOperationResult();

        public Task<OperationResult<ProposalDateDto>> GetAllDatesAsync()
            => GetProposalDates()
                .ToResult("No hay ninguna propuesta.")
                .Map(dates => new ProposalDateDto(dates))
                .FinallyOperationResult();


        public Task<OperationResult<ProposalFullDto>> GetByIdAsync(Guid id)
            => GetProposalById(id, true)
                .ToResult("No existe una propuesta con ese id.")
                .Map(ConvertToFullDto)
                .FinallyOperationResult();


        public async Task<OperationResult> DeleteAsync(Guid id)
            => await GetProposalById(id, false)
                .ToResult("No existe una propuesta con ese id.")
                .Tap(RemoveProposalAsync)
                .FinallyOperationResult();


        public Task<OperationResult<ProposalFullDto>> CreateAsync(CreateProposalInput input)
            => Validate(input)
                    .Map(CreateProposalAsync)
                    .Check(async proposal => await _emailManager.SendEmailAsync(await proposal.ConvertToEmailInput()))
                    .FinallyOperationResult();


        public async Task<OperationResult> DeleteAllAsync()
            => await GetProposals(new(), false)
                    .ToResult("No hay ninguna propuesta.")
                    .Tap(RemoveProposalAsync)
                    .FinallyOperationResult();


        private async Task<Maybe<List<Proposal>>> GetProposals(ProposalFilter filter, bool include)
            => await _dbContext.Proposals.AsNoTracking()
                    .WhereIf(!string.IsNullOrWhiteSpace(filter.Title), x => x.Title.ToLower() == filter.Title.ToLower())
                    .WhereIf(filter.Date.HasValue, x => x.EventDate.Date == filter.Date.Value)
                    .WhereIf(filter.IsActive.HasValue, x => x.IsActive == filter.IsActive.Value)
                    .IncludeIf(include, x => x.Speakers).ToListAsync();


        private async Task<Maybe<List<DateTime>>> GetProposalDates()
            => await _dbContext.Proposals.AsNoTracking().Select(x => x.EventDate).ToListAsync();


        private async Task<Maybe<Proposal>> GetProposalById(Guid id, bool include)
            => await _dbContext.Proposals.IncludeIf(include, x => x.Speakers).SingleOrDefaultAsync(x => x.Id == id);


        private Result<CreateProposalInput> Validate(CreateProposalInput input)
        {
            CreateProposalValidator validator = new(_dbContext);

            var validationResult = validator.Validate(input);

            return validationResult.ToResult(input);

        }

        private async Task<ProposalFullDto> CreateProposalAsync(CreateProposalInput input)
        {
            Proposal proposal = new(input.Title, input.Description, input.AudienceAnswer, input.KnowledgeAnswer, input.UseCaseAnswer, input.Date);

            List<SpeakerDto> speakerdtos = new();

            foreach (var speakerInput in input.Speakers)
            {
                Speaker speaker = new(speakerInput.Name, speakerInput.LastName, speakerInput.Email, speakerInput.Twitter, speakerInput.Description, speakerInput.Image);

                proposal.Speakers.Add(speaker);

                speakerdtos.Add(speaker.ConvertToDto());
            }

            await _dbContext.AddAsync(proposal);

            await _dbContext.SaveChangesAsync();

            ProposalFullDto proposalFullDto = new(proposal.ConvertToDto(), speakerdtos); ;

            return proposalFullDto;
        }

        private async Task RemoveProposalAsync(Proposal proposal)
        {
            if(proposal.Speakers is not null)
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
