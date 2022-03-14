
using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Speakers;

using System;
using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.Proposals
{
    record CreateProposalInput(string Title, string Description, DateTime Date, string AudienceAnswer, string KnowledgeAnswer, string UseCaseAnswer, IEnumerable<CreateSpeakerInput> Speakers);
}
