using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Speakers;

using System;
using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals
{
    record UpdateProposalInput(Guid Id, string Title, string Description, DateTime Date, string AudienceAnswer, string KnowledgeAnswer, string UseCaseAnswer, int? WebinarNumber, Uri? Meetup, Uri? Streamyard, Uri? LiveStreaming, Uri? Flyer, IEnumerable<UpdateSpeakerInput> Speakers);
}
