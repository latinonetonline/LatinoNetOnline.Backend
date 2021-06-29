using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Shared.Abstractions.Modules;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Services
{
    interface IProposalService
    {
        Task<OperationResult<ProposalFullDto>> GetByIdAsync(GetProposalInput input);
    }

    class ProposalService : IProposalService
    {
        private readonly IModuleClient _moduleClient;

        public ProposalService(IModuleClient moduleClient)
        {
            _moduleClient = moduleClient;
        }

        public Task<OperationResult<ProposalFullDto>> GetByIdAsync(GetProposalInput input)
            => _moduleClient.GetAsync<OperationResult<ProposalFullDto>>("modules/proposals/get", input);
    }
}
