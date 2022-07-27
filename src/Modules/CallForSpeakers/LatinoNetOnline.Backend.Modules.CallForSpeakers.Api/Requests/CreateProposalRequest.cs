using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Speakers;

using System;
using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.Webinars.Api.Requests
{
    record CreateProposalRequest(IEnumerable<CreateSpeakerInput> Speakers, string Title, string Description, DateTime Date, string AudienceAnswer, string KnowledgeAnswer, string UseCaseAnswer);

}
