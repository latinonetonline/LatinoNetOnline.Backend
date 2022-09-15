using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Extensions;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Services;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;
using LatinoNetOnline.Backend.Shared.Commons.Results;
using LatinoNetOnline.GenericRepository.Repositories;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using static System.Net.Mime.MediaTypeNames;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.ChangeProposalFlyer
{
    public class ChangeProposalFlyerRequestHandler : IRequestHandler<ChangeProposalFlyerRequest, Result<ChangeProposalFlyerResponse>>
    {
        private readonly IRepository<Proposal> _repository;
        private readonly IStorageService _storageService;
        public ChangeProposalFlyerRequestHandler(IRepository<Proposal> repository, IStorageService storageService)
        {
            _repository = repository;
            _storageService = storageService;
        }

        public async Task<Result<ChangeProposalFlyerResponse>> Handle(ChangeProposalFlyerRequest request, CancellationToken cancellationToken)
        {
            var proposal = await _repository.SingleOrDefaultAsync(x => x.Id == request.Id, true, cancellationToken);

            if (proposal is null)
            {
                return "No hay ninguna propuesta.";
            }

            var imageResult = await _storageService.UploadFile("flyers", Guid.NewGuid().ToString() + ".jpg", request.Image);


            if (imageResult.IsSuccess)
            {
                proposal.Flyer = imageResult.Result;

                await _repository.UpdateAsync(proposal, cancellationToken);

            }

            return new ChangeProposalFlyerResponse(proposal.Id, proposal.Title, proposal.Description, proposal.EventDate, proposal.CreationTime, proposal.AudienceAnswer, proposal.KnowledgeAnswer, proposal.UseCaseAnswer, proposal.IsActive, proposal.WebinarNumber, proposal.Status, proposal.Meetup, proposal.Streamyard, proposal.LiveStreaming, proposal.Flyer, proposal.Views, proposal.LiveAttends);
        }
    }
}
