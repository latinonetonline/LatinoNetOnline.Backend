using System;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Api.Requests
{
    record CreateProposalRequest(string Name, string LastName, string Email, string Twitter, string SpeakerDescription, string ProposalTitle, string ProposalDescription, DateTime Date, string AudienceAnswer, string KnowledgeAnswer, string UseCaseAnswer);

}
