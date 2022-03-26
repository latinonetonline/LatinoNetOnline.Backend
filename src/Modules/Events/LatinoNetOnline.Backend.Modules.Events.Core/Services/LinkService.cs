using LatinoNetOnline.Backend.Modules.Events.Core.Dto.GitHub;
using LatinoNetOnline.Backend.Modules.Events.Core.Entities;
using LatinoNetOnline.Backend.Modules.Events.Core.Managers;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Services
{
    interface ILinkService
    {
        Task<OperationResult<Link>> Create(Link link);
        Task<OperationResult> Delete(string name);
        Task<OperationResult<Link>> Get(string name);
        Task<OperationResult<IEnumerable<Link>>> GetAll();
        Task<OperationResult<Link>> Update(Link link);
    }

    class LinkService : ILinkService
    {
        private readonly IGitHubService _githubService;

        public LinkService(IGitHubService githubService)
        {
            _githubService = githubService;
        }

        public async Task<OperationResult<Link>> Create(Link link)
        {
            await _githubService.CreateFileAsync(260336124, "links", link.Name, JsonSerializer.Serialize(link));
            return OperationResult<Link>.Success(link);
        }

        public async Task<OperationResult<Link>> Update(Link link)
        {
            await _githubService.UpdateFileAsync(260336124, "links", link.Name, JsonSerializer.Serialize(link));

            return OperationResult<Link>.Success(link);
        }

        public async Task<OperationResult> Delete(string name)
        {
            await _githubService.DeleteFileAsync(260336124, "links", name);
            return OperationResult.Success();
        }

        public async Task<OperationResult<IEnumerable<Link>>> GetAll()
        {
            IEnumerable<GhFileContent> files = await _githubService.GetAllFilesWithContentAsync(260336124, "links");
            if (files is null)
            {
                return OperationResult<IEnumerable<Link>>.Success(Enumerable.Empty<Link>());
            }
            else
            {
                return OperationResult<IEnumerable<Link>>.Success(files.Select(x => JsonSerializer.Deserialize<Link>(x.Content)));
            }

        }

        public async Task<OperationResult<Link>> Get(string name)
        {
            GhFileContent? fileContent = await _githubService.GetFileContentAsync(260336124, "links", name);
            var link = JsonSerializer.Deserialize<Link>(fileContent.Content);
            return OperationResult<Link>.Success(link);
        }
    }
}
