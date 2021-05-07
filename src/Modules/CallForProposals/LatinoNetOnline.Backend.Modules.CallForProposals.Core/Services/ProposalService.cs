using CSharpFunctionalExtensions;

using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Data;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Speakers;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Entities;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Extensions;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Managers;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Validators;
using LatinoNetOnline.Backend.Shared.Abstractions.OperationResults;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Services
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
            => GetProposals(filter)
                .ToResult("No hay ninguna propuesta.")
                .Map(ConvertToFullDto)
                .FinallyOperationResult();

        public Task<OperationResult<ProposalDateDto>> GetAllDatesAsync()
            => GetProposalDates()
                .ToResult("No hay ninguna propuesta.")
                .Map(dates => new ProposalDateDto(dates))
                .FinallyOperationResult();


        public Task<OperationResult<ProposalFullDto>> GetByIdAsync(Guid id)
            => GetProposalById(id)
                .ToResult("No existe una propuesta con ese id.")
                .Map(ConvertToFullDto)
                .FinallyOperationResult();


        public async Task<OperationResult> DeleteAsync(Guid id)
            => await GetProposalById(id)
                .ToResult("No existe una propuesta con ese id.")
                .Tap(RemoveProposalAsync)
                .FinallyOperationResult();

        public Task<OperationResult<ProposalFullDto>> CreateAsync(CreateProposalInput input)
            => Validate(input)
                    .Map(CreateProposalAsync)
                    .Check(async proposal => await _emailManager.SendEmailAsync(await proposal.ConvertToEmailInput()))
                    .FinallyOperationResult();


        private async Task<Maybe<List<Proposal>>> GetProposals(ProposalFilter filter)
        {
            var query = _dbContext.Proposals.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter.Title))
                query = query.Where(x => x.Title.ToLower() == filter.Title.ToLower());

            if (filter.Date.HasValue)
                query = query.Where(x => x.EventDate.Date == filter.Date.Value);

            if (filter.IsActive.HasValue)
                query = query.Where(x => x.IsActive == filter.IsActive.Value);

            return await query.Include(x => x.Speakers).ToListAsync();

        }
        private async Task<Maybe<List<DateTime>>> GetProposalDates()
        {
            return await _dbContext.Proposals.AsNoTracking().Select(x => x.EventDate).ToListAsync();
        }

        private async Task<Maybe<Proposal>> GetProposalById(Guid id)
        {
            return await _dbContext.Proposals.Include(x => x.Speakers).SingleOrDefaultAsync(x => x.Id == id);
        }

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

            foreach (Speaker speaker in proposal.Speakers)
            {
                _dbContext.Speakers.Remove(speaker);
            }

            _dbContext.Proposals.Remove(proposal);
            await _dbContext.SaveChangesAsync();
        }

        private async Task RemoveProposalAsync(IEnumerable<Proposal> proposals)
        {

            foreach (var item in proposals)
            {
                await RemoveProposalAsync(item);
            }

        }

        private Proposal ConvertToEntity(CreateProposalInput input)
            => new()
            {
                Title = input.Title,
                Description = input.Description,
                EventDate = input.Date,
                CreationTime = DateTime.Now,
                AudienceAnswer = input.AudienceAnswer,
                KnowledgeAnswer = input.KnowledgeAnswer,
                UseCaseAnswer = input.UseCaseAnswer
            };

        private ProposalDto ConvertToDto(Proposal proposal)
            => proposal.ConvertToDto();

        public async Task<OperationResult> DeleteAllAsync()
            => await GetProposals(new())
                    .ToResult("No hay ninguna propuesta.")
                    .Tap(RemoveProposalAsync)
                    .FinallyOperationResult();

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
