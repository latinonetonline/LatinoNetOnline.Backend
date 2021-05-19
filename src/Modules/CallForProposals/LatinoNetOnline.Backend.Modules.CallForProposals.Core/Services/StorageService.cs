using AivenEcommerce.V1.Modules.GitHub.Services;

using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using System;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Services
{
    interface IStorageService
    {
        Task<OperationResult<Uri>> UploadFile(string path, string filename, byte[] file);
    }

    class StorageService : IStorageService
    {
        private readonly IGitHubService _githubService;

        public StorageService(IGitHubService githubService)
        {
            _githubService = githubService ?? throw new ArgumentNullException(nameof(githubService));
        }

        public async Task<OperationResult<Uri>> UploadFile(string path, string filename, byte[] file)
        {
            var fileContent = await _githubService.CreateFileAsync(363513177, path, filename, file);

            return OperationResult<Uri>.Success(new(fileContent.DownloadUrl));
        }
    }
}
