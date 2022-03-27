using LatinoNetOnline.Backend.Modules.Events.Core.Dto.GitHub;
using LatinoNetOnline.Backend.Modules.Events.Core.Entities;
using LatinoNetOnline.Backend.Modules.Events.Core.Managers;
using LatinoNetOnline.Backend.Modules.Events.Core.Options;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.Extensions.Options;

using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Services
{
    interface ILinkService
    {
        Task<OperationResult<Link>> CreateAsync(Link link);
        Task<OperationResult> DeleteAsync(string name);
        Task<OperationResult<Link>> GetAsync(string name);
        Task<OperationResult<IEnumerable<Link>>> GetAllAsync();
        Task<OperationResult<Link>> UpdateAsync(Link link);
    }

    class LinkService : ILinkService
    {
        private readonly IGitHubService _githubService;
        private readonly GithubOptions _options;
        const string PATH = "links";

        public LinkService(IGitHubService githubService, IOptions<GithubOptions> options)
        {
            _githubService = githubService;
            _options = options.Value;
        }

        public async Task<OperationResult<Link>> CreateAsync(Link link)
        {
            await _githubService.CreateFileAsync(_options.LinkRepositoryId, PATH, link.Name, JsonSerializer.Serialize(link));
            return OperationResult<Link>.Success(link);
        }

        public async Task<OperationResult<Link>> UpdateAsync(Link link)
        {
            await _githubService.UpdateFileAsync(_options.LinkRepositoryId, PATH, link.Name, JsonSerializer.Serialize(link));

            return OperationResult<Link>.Success(link);
        }

        public async Task<OperationResult> DeleteAsync(string name)
        {
            await _githubService.DeleteFileAsync(_options.LinkRepositoryId, PATH, name);
            return OperationResult.Success();
        }

        public async Task<OperationResult<IEnumerable<Link>>> GetAllAsync()
        {
            IEnumerable<GhFileContent> files = await _githubService.GetAllFilesWithContentAsync(_options.LinkRepositoryId, PATH);
            if (files is null)
            {
                return OperationResult<IEnumerable<Link>>.Success(Enumerable.Empty<Link>());
            }
            else
            {
                var links = files.Where(x => !string.IsNullOrWhiteSpace(x.Content)).Select(x => JsonSerializer.Deserialize<Link>(x.Content!)!);

                return OperationResult<IEnumerable<Link>>.Success(links);
            }

        }

        public async Task<OperationResult<Link>> GetAsync(string name)
        {
            GhFileContent? fileContent = await _githubService.GetFileContentAsync(_options.LinkRepositoryId, PATH, name);

            if (fileContent is not null && !string.IsNullOrWhiteSpace(fileContent.Content))
            {
                var link = JsonSerializer.Deserialize<Link>(fileContent.Content);

                if(link is not null)
                {
                    return OperationResult<Link>.Success(link);
                }
            }

            return OperationResult<Link>.Fail("No se pudo encontrar el Link.");
        }
    }
}
