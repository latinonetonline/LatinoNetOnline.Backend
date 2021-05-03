using CSharpFunctionalExtensions;

using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Data;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Entities;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Extensions;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Validators;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Services
{
    interface IProposalService
    {
        Task<Result<ProposalDto>> CreateAsync(CreateProposalInput input);
        Task<Result> DeleteAsync(Guid id);
        Task<Result> DeleteAllAsync();
        Task<Result<IEnumerable<ProposalFullDto>>> GetAllAsync();
        Task<Result<ProposalDateDto>> GetAllDatesAsync();
        Task<Result<ProposalFullDto>> GetByIdAsync(Guid id);
    }

    class ProposalService : IProposalService
    {
        private readonly ApplicationDbContext _dbContext;

        public ProposalService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<IEnumerable<ProposalFullDto>>> GetAllAsync()
            => await GetProposals()
                .ToResult("No hay ninguna propuesta.")
                .Map(proposals => proposals.Select(x => x.ConvertToFullDto()));

        public async Task<Result<ProposalDateDto>> GetAllDatesAsync()
            => await GetProposalDates()
                .ToResult("No hay ninguna propuesta.")
                .Map(dates => new ProposalDateDto(dates));


        public async Task<Result<ProposalFullDto>> GetByIdAsync(Guid id)
            => await GetProposalById(id)
                .ToResult("No existe una propuesta con ese id.")
                .Map(proposal => proposal.ConvertToFullDto());


        public async Task<Result> DeleteAsync(Guid id)
            => await GetProposalById(id)
                .ToResult("No existe una propuesta con ese id.")
                .Tap(RemoveProposalAsync);


        public async Task<Result<ProposalDto>> CreateAsync(CreateProposalInput input)
            => await Validate(input)
                    .Map(ConvertToEntity)
                    .Map(AddProposalAsync)
                    .Map(ConvertToDto);



        private async Task<Maybe<List<Proposal>>> GetProposals()
        {
            return await _dbContext.Proposals.Include(x => x.Speaker).ToListAsync();
        }
        private async Task<Maybe<List<DateTime>>> GetProposalDates()
        {
            return await _dbContext.Proposals.Select(x => x.EventDate).ToListAsync();
        }

        private async Task<Maybe<Proposal>> GetProposalById(Guid id)
        {
            return await _dbContext.Proposals.Include(x => x.Speaker).SingleOrDefaultAsync(x => x.Id == id);
        }

        private Result<CreateProposalInput> Validate(CreateProposalInput input)
        {
            CreateProposalValidator validator = new();

            var validationResult = validator.Validate(input);

            return validationResult.ToResult(input);

        }

        private async Task<Proposal> AddProposalAsync(Proposal proposal)
        {
            await _dbContext.Proposals.AddAsync(proposal);
            await _dbContext.SaveChangesAsync();

            return proposal;
        }

        private async Task RemoveProposalAsync(Proposal proposal)
        {
            var speaker = await _dbContext.Speakers.FindAsync(proposal.SpeakerId);
            _dbContext.Speakers.Remove(speaker);
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
                SpeakerId = input.SpeakerId,
                EventDate = input.Date,
                CreationTime = DateTime.Now,
                AudienceAnswer = input.AudienceAnswer,
                KnowledgeAnswer = input.KnowledgeAnswer,
                UseCaseAnswer = input.UseCaseAnswer
            };

        private ProposalDto ConvertToDto(Proposal proposal)
            => proposal.ConvertToDto();

        public async Task<Result> DeleteAllAsync()
            =>  await GetProposals()
                    .ToResult("No hay ninguna propuesta.")
                    .Tap(RemoveProposalAsync);

    }
}
