namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Speakers
{
    record CreateSpeakerInput(string Name, string LastName, string Email, string Twitter, string Description, byte[] Image);
}
