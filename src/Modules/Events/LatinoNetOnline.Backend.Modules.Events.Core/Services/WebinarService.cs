using CSharpFunctionalExtensions;

using LatinoNetOnline.Backend.Modules.Events.Core.Data;
using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Webinars;
using LatinoNetOnline.Backend.Modules.Events.Core.Entities;
using LatinoNetOnline.Backend.Modules.Events.Core.Extensions;
using LatinoNetOnline.Backend.Modules.Events.Core.Validators;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Services
{
    interface IWebinarService
    {
        Task<OperationResult<WebinarDto>> CreateAsync(CreateWebinarInput input);
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

        public WebinarService(EventDbContext dbContext, IMeetupService meetupService, IProposalService proposalService)
        {
            _dbContext = dbContext;
            _meetupService = meetupService;
            _proposalService = proposalService;
        }

        public Task<OperationResult<WebinarDto>> CreateAsync(CreateWebinarInput input)
            => Validate(input)
                .Map(ConvertToEntity)
                .Map(AddWebinarAsync)
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

        private async Task<Maybe<IEnumerable<Webinar>>> GetAllWebinars()
            => await _dbContext.Webinars
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

        private async Task<Webinar> ConvertToEntity(CreateWebinarInput input)
        {
            int? maxWebinarNumber = await _dbContext.Webinars.MaxNumberAsync();
            input.Number = maxWebinarNumber.GetValueOrDefault() + 1;

            var webinar = input.ConvertToEntity();

            var meetup = await _meetupService.CreateEventAsync(new(input.Title, input.Description, input.StartDateTime));

            if (meetup.IsSuccess && meetup.Result.Id is not null)
            {
                webinar.MeetupId = long.Parse(meetup.Result.Id.Replace("!chp", ""));
                webinar.Status = Enums.WebinarStatus.Draft;
            }
                

            return webinar;
        }

        private WebinarDto ConvertToDto(Webinar webinar)
            => webinar.ConvertToDto();

        private IEnumerable<WebinarDto> ConvertToDto(IEnumerable<Webinar> webinars)
            => webinars.Select(ConvertToDto);


        private async Task RemoveWebinarAsync(Webinar webinar)
        {
            _dbContext.Webinars.Remove(webinar);
            await _dbContext.SaveChangesAsync();
        }
    }
}
