using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Speakers;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Extensions;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Managers;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;
using LatinoNetOnline.Backend.Shared.Commons.Results;
using LatinoNetOnline.GenericRepository.Repositories;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.CreateProposal
{
    internal class CreateProposalRequestHandler : IRequestHandler<CreateProposalRequest, Result<ProposalFullDto>>
    {
        private readonly IRepository<Proposal> _proposalRepository;
        private readonly IRepository<Speaker> _speakerRepository;
        private readonly IHttpContextAccessor _accessor;
        private readonly IEmailManager _emailManager;

        public CreateProposalRequestHandler(IRepository<Proposal> proposalRepository, IRepository<Speaker> speakerRepository, IHttpContextAccessor accessor, IEmailManager emailManager)
        {
            _proposalRepository = proposalRepository;
            _speakerRepository = speakerRepository;
            _accessor = accessor;
            _emailManager = emailManager;
        }

        public async Task<Result<ProposalFullDto>> Handle(CreateProposalRequest request, CancellationToken cancellationToken)
        {

            Proposal proposal = new(request.Title, request.Description, request.AudienceAnswer, request.KnowledgeAnswer, request.UseCaseAnswer, request.Date);
            List<SpeakerDto> speakerdtos = new();

            var userId = new Guid(_accessor.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var userEmail = _accessor.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email).Value;


            var speakers = await _speakerRepository.FindAsync(x => x.UserId == userId || request.Speakers.Select(s => s.Email).Contains(x.Email));

            foreach (var speakerInput in request.Speakers)
            {
                Guid newUserId = userEmail == speakerInput.Email ? userId : Guid.Empty;

                if (speakers.Select(x => x.Email).Distinct().Contains(speakerInput.Email))
                {
                    Speaker speaker = speakers.First(x => x.UserId == userId || x.Email == speakerInput.Email);
                    speaker.Name = speakerInput.Name;
                    speaker.LastName = speakerInput.LastName;
                    speaker.LastName = speakerInput.LastName;
                    speaker.Twitter = speakerInput.Twitter;
                    speaker.Description = speakerInput.Description;
                    speaker.Image = speakerInput.Image;

                    if (speaker.UserId == Guid.Empty)
                        speaker.UserId = newUserId;

                    proposal.Speakers.Add(speaker);

                    speakerdtos.Add(speaker.ConvertToDto());
                }
                else
                {
                    Speaker speaker = new(speakerInput.Name, speakerInput.LastName, speakerInput.Email, speakerInput.Twitter, speakerInput.Description, speakerInput.Image, newUserId);

                    proposal.Speakers.Add(speaker);

                    speakerdtos.Add(speaker.ConvertToDto());
                }

            }

            await SetWebinarNumberAsync(proposal);

            await _proposalRepository.AddAsync(proposal, cancellationToken);



            ProposalFullDto proposalFullDto = new(proposal.ConvertToDto(), speakerdtos);


            await _emailManager.SendEmailAsync(await proposalFullDto.ConvertToProposalCreatedEmailInput());

            return proposalFullDto;


        }

        private async Task<Proposal> SetWebinarNumberAsync(Proposal webinar)
        {
            int? maxWebinarNumber = await _proposalRepository.Query(false).MaxNumberAsync();
            webinar.WebinarNumber = maxWebinarNumber.GetValueOrDefault() + 1;

            return webinar;
        }
    }
}
