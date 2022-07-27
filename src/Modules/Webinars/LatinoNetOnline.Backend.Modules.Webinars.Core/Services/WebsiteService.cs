using LatinoNetOnline.Backend.Modules.Webinars.Core.Data;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Services
{
    interface IWebsiteService
    {
        Task<OperationResult<ProposalPublicDto>> GetNextWebinars();
        Task<OperationResult<IEnumerable<ProposalPublicDto>>> GetPastWebinars();
    }

    class WebsiteService : IWebsiteService
    {
        private readonly ApplicationDbContext _dbContext;

        public WebsiteService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IEnumerable<ProposalPublicDto>>> GetPastWebinars()
        {
            var proposals = await _dbContext.Proposals.AsNoTracking()
                .Where(x => x.Status == Enums.WebinarStatus.Published && x.EventDate.Date < DateTime.Today)
                .OrderByDescending(x => x.EventDate)
                .Take(3)
                .Select(x => new ProposalPublicDto(x.Title, x.Description, x.Flyer!, x.LiveStreaming!, x.Meetup!, x.EventDate))
                .ToListAsync();


            return OperationResult<IEnumerable<ProposalPublicDto>>.Success(proposals);

        }

        public async Task<OperationResult<ProposalPublicDto>> GetNextWebinars()
        {
            var proposal = await _dbContext.Proposals.AsNoTracking()
                .Where(x => x.Status == Enums.WebinarStatus.Published && x.EventDate.Date >= DateTime.Today)
                .OrderByDescending(x => x.EventDate)
                .Select(x => new ProposalPublicDto(x.Title, x.Description, x.Flyer!, x.LiveStreaming!, x.Meetup!, x.EventDate))
                .FirstOrDefaultAsync();


            return OperationResult<ProposalPublicDto>.Success(proposal);

        }
    }
}
