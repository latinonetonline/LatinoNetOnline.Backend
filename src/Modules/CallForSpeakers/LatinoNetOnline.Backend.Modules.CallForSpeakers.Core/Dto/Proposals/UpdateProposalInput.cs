﻿using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Speakers;

using System;
using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Proposals
{
    record UpdateProposalInput(Guid Id, string Title, string Description, DateTime Date, string AudienceAnswer, string KnowledgeAnswer, string UseCaseAnswer, IEnumerable<UpdateSpeakerInput> Speakers);
}