using AivenEcommerce.V1.Modules.GitHub.Services;

using System;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Services
{
    interface IStorageService
    {
        Task<Uri> UploadFile(string path, string filename, byte[] file);
    }

    class StorageService : IStorageService
    {
        private readonly IGitHubService _githubService;

        public StorageService(IGitHubService githubService)
        {
            _githubService = githubService ?? throw new ArgumentNullException(nameof(githubService));
        }

        public async Task<Uri> UploadFile(string path, string filename, byte[] file)
        {
            var fileContent = await _githubService.CreateFileAsync(363513177, path, filename, file);

            return new(fileContent.DownloadUrl);
        }
    }
}
