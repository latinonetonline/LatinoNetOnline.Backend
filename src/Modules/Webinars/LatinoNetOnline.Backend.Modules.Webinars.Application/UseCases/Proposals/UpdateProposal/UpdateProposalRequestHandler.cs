using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Speakers;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Extensions;
using LatinoNetOnline.Backend.Shared.Commons.Results;
using LatinoNetOnline.GenericRepository.Repositories;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.UpdateProposal
{
    public class UpdateProposalRequestHandler : IRequestHandler<UpdateProposalRequest, Result<UpdateProposalResponse>>
    {
        private readonly IRepository<Proposal> _proposalRepository;
        private readonly IRepository<Speaker> _speakerRepository;

        public UpdateProposalRequestHandler(IRepository<Proposal> proposalRepository, IRepository<Speaker> speakerRepository)
        {
            _proposalRepository = proposalRepository;
            _speakerRepository = speakerRepository;
        }

        public async Task<Result<UpdateProposalResponse>> Handle(UpdateProposalRequest request, CancellationToken cancellationToken)
        {
            Proposal proposal = await _proposalRepository.Query(true).Include(x => x.Speakers).SingleAsync(x => x.Id == request.Id, cancellationToken);

            proposal.Title = request.Title;
            proposal.Description = request.Description;
            proposal.AudienceAnswer = request.AudienceAnswer;
            proposal.KnowledgeAnswer = request.KnowledgeAnswer;
            proposal.UseCaseAnswer = request.UseCaseAnswer;
            proposal.WebinarNumber = request.WebinarNumber;
            proposal.Meetup = request.Meetup;
            proposal.Streamyard = request.Streamyard;
            proposal.LiveStreaming = request.LiveStreaming;
            proposal.Flyer = request.Flyer;
            proposal.EventDate = request.Date;
            proposal.Views = request.Views;
            proposal.LiveAttends = request.LiveAttends;
            proposal.Speakers = new List<Speaker>();

            List<SpeakerDto> speakerdtos = new();

            foreach (var speakerInput in request.Speakers)
            {

                Speaker? speaker = await _speakerRepository.SingleOrDefaultAsync(x => x.Id == speakerInput.Id, true, cancellationToken);

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

            await _proposalRepository.UpdateAsync(proposal, cancellationToken);



            UpdateProposalResponse response = new(proposal.ConvertToDto(), speakerdtos);

            return response;
        }
    }
}
