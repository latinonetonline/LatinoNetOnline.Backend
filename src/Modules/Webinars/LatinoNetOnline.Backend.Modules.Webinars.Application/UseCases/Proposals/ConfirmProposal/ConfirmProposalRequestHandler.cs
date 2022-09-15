using LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.ChangeProposalFlyer;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Enums;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Extensions;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Managers;
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

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.ConfirmProposal
{
    public class ConfirmProposalRequestHandler : IRequestHandler<ConfirmProposalRequest, Result<ConfirmProposalResponse>>
    {
        private readonly IRepository<Proposal> _repository;
        private readonly IEmailManager _emailManager;

        public ConfirmProposalRequestHandler(IRepository<Proposal> repository, IEmailManager emailManager)
        {
            _repository = repository;
            _emailManager = emailManager;
        }

        public async Task<Result<ConfirmProposalResponse>> Handle(ConfirmProposalRequest request, CancellationToken cancellationToken)
        {
            var proposal = await _repository.Query(true).Include(x => x.Speakers).SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (proposal is not null)
            {
                proposal.Status = WebinarStatus.Published;

                await _repository.UpdateAsync(proposal, cancellationToken);


                ProposalFullDto proposalFullDto = new(proposal.ConvertToDto(), proposal.Speakers.Select(x => x.ConvertToDto()));


                var emailInput = await proposalFullDto.ConvertToProposalConfirmedEmailInput();

                var emailResult = await _emailManager.SendEmailAsync(emailInput);

                if (!emailResult.IsSuccess)
                {
                    return "Hubo un error al enviar el Email.";

                }

                return new ConfirmProposalResponse(proposal.Id, proposal.Title, proposal.Description, proposal.EventDate, proposal.CreationTime, proposal.AudienceAnswer, proposal.KnowledgeAnswer, proposal.UseCaseAnswer, proposal.IsActive, proposal.WebinarNumber, proposal.Status, proposal.Meetup, proposal.Streamyard, proposal.LiveStreaming, proposal.Flyer, proposal.Views, proposal.LiveAttends);
            }

            return "No existe la propuesta";
        }
    }
}
