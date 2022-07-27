using System;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals
{
    public record ProposalPublicDto(string Title, string Description, Uri Flyer, Uri LiveStreaming, Uri Meetup, DateTime EventDate);
}
