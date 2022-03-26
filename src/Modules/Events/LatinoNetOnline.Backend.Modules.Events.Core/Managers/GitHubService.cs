using LatinoNetOnline.Backend.Modules.Events.Core.Dto.GitHub;

using Octokit;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Managers
{
    internal interface IGitHubService
    {
        Task<GhFileContent> CreateFileAsync(long repositoryId, string path, string fileName, string content);
        Task<GhFileContent> CreateFileAsync(long repositoryId, string path, string fileName, byte[] content);
        Task<GhFileContent?> GetFileContentAsync(long repositoryId, string path, string fileName);
        Task<bool> ExistFileAsync(long repositoryId, string path, string fileName);
        Task<IEnumerable<GhFileContent>> GetAllFilesAsync(long repositoryId, string path);
        Task<IEnumerable<GhFileContent>> GetAllDirectoriesAsync(long repositoryId, string path);
        Task<IEnumerable<GhFileContent>> GetAllFilesWithContentAsync(long repositoryId, string path);
        Task DeleteFileAsync(long repositoryId, string path, string fileName);
        Task UpdateFileAsync(long repositoryId, string path, string fileName, string content);
    }

    internal class GitHubService : IGitHubService
    {
        private readonly IGitHubClient _githubClient;

        public GitHubService(IGitHubClient githubClient)
        {
            _githubClient = githubClient;
        }

        public async Task<GhFileContent> CreateFileAsync(long repositoryId, string path, string fileName, string content)
        {
            RepositoryContentChangeSet response = await _githubClient.Repository.Content.CreateFile(
                    repositoryId,
                    Path.Combine(path, fileName),
                    new($"Create {fileName}",
                                          content,
                                          "main"));

            return new GhFileContent(fileName, content, response.Content.DownloadUrl);
        }

        public async Task<GhFileContent> CreateFileAsync(long repositoryId, string path, string fileName, byte[] content)
        {
            string contentBase64 = Convert.ToBase64String(content);

            RepositoryContentChangeSet response = await _githubClient.Repository.Content.CreateFile(
                    repositoryId,
                    Path.Combine(path, fileName),
                    new($"Create {fileName}",
                                          contentBase64,
                                          "main",
                                          false));

            return new GhFileContent(fileName, response.Content.DownloadUrl);
        }

        public async Task DeleteFileAsync(long repositoryId, string path, string fileName)
        {
            IReadOnlyList<RepositoryContent> contents = await _githubClient.Repository.Content.GetAllContents(repositoryId, Path.Combine(path, fileName));
            RepositoryContent repositoryContent = contents[0];

            await _githubClient.Repository.Content.DeleteFile(
                repositoryId,
                    Path.Combine(path, fileName),
                    new($"Delete {fileName}",
                                          repositoryContent.Sha,
                                          "main")
                    );
        }

        public async Task UpdateFileAsync(long repositoryId, string path, string fileName, string content)
        {
            IReadOnlyList<RepositoryContent> contents = await _githubClient.Repository.Content.GetAllContents(repositoryId, Path.Combine(path, fileName));
            RepositoryContent repositoryContent = contents[0];

            await _githubClient.Repository.Content.UpdateFile(
                repositoryId,
                    Path.Combine(path, fileName),
                    new($"Update {fileName}",
                                          content,
                                          repositoryContent.Sha,
                                          "main")
                    );
        }

        public async Task<bool> ExistFileAsync(long repositoryId, string path, string fileName)
        {
            GhFileContent? fileContent = await GetFileContentAsync(repositoryId, path, fileName);
            return fileContent != null;
        }

        public async Task<IEnumerable<GhFileContent>> GetAllFilesAsync(long repositoryId, string path)
        {
            try
            {
                IReadOnlyList<RepositoryContent> contents = await _githubClient.Repository.Content.GetAllContents(repositoryId, path);

                var files = contents.Where(x => x.Type == new StringEnum<ContentType>(ContentType.File)).Select(x => new GhFileContent(x.Name, x.Content, x.DownloadUrl));

                return files;
            }
            catch (NotFoundException)
            {
                return Enumerable.Empty<GhFileContent>();
            }
        }

        public async Task<GhFileContent?> GetFileContentAsync(long repositoryId, string path, string fileName)
        {
            try
            {
                IReadOnlyList<RepositoryContent> contents = await _githubClient.Repository.Content.GetAllContents(repositoryId, Path.Combine(path, fileName));
                RepositoryContent content = contents[0];
                return new(content.Name, content.Content, content.DownloadUrl);
            }
            catch (NotFoundException)
            {
                return null;
            }
        }

        public async Task<IEnumerable<GhFileContent>> GetAllFilesWithContentAsync(long repositoryId, string path)
        {
            try
            {
                IReadOnlyList<RepositoryContent> contents = await _githubClient.Repository.Content.GetAllContents(repositoryId, path);

                var files = contents.Where(x => x.Type == new StringEnum<ContentType>(ContentType.File)).Select(x => new GhFileContent(x.Name, x.Content, x.DownloadUrl)).ToList();

                for (int i = 0; i < files.Count; i++)
                {
                    var fileContent = await GetFileContentAsync(repositoryId, path, files[i].Name);

                    files[i].Content = fileContent?.Content;
                }

                return files;
            }
            catch (NotFoundException)
            {
                return Enumerable.Empty<GhFileContent>();
            }
        }

        public async Task<IEnumerable<GhFileContent>> GetAllDirectoriesAsync(long repositoryId, string path)
        {
            try
            {
                IReadOnlyList<RepositoryContent> contents = await _githubClient.Repository.Content.GetAllContents(repositoryId, path);

                var files = contents.Where(x => x.Type == new StringEnum<ContentType>(ContentType.Dir)).Select(x => new GhFileContent(x.Name, x.Content, x.DownloadUrl));

                return files;
            }
            catch (NotFoundException)
            {
                return Enumerable.Empty<GhFileContent>();
            }
        }
    }
}
