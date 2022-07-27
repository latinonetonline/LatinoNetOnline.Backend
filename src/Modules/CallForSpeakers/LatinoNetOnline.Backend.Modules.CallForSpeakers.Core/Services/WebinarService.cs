using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Webinars;
using LatinoNetOnline.Backend.Shared.Abstractions.Modules;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Services
{
    interface IWebinarService
    {
        Task<OperationResult<WebinarDto>> GetAsync(GetWebinarInput input);
    }

    class WebinarService : IWebinarService
    {
        private readonly IModuleClient _moduleClient;

        public WebinarService(IModuleClient moduleClient)
        {
            _moduleClient = moduleClient;
        }

        public Task<OperationResult<WebinarDto>> GetAsync(GetWebinarInput input)
            => _moduleClient.GetAsync<OperationResult<WebinarDto>>("modules/webinars/get", input);
    }
}
