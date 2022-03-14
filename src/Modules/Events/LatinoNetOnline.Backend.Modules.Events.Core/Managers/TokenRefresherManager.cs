using AivenEcommerce.V1.Modules.GitHub.Services;

using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Tokens;

using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Managers
{
    interface ITokenRefresherManager
    {
        Task<Token> GetMeetupTokenAsync();
        Task<Token> GetGoogleTokenAsync();
    }

    class TokenRefresherManager : ITokenRefresherManager
    {
        private readonly IGitHubService _githubService;

        const long REPOSITORY_ID = 382245667;
        const string FOLDER_PATH = "refresh-tokens";

        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };


        public TokenRefresherManager(IGitHubService githubService)
        {
            _githubService = githubService;
        }

        public Task<Token> GetMeetupTokenAsync()
            => GetTokenAsync("meetup.json");

        public Task<Token> GetGoogleTokenAsync()
            => GetTokenAsync("google.json");

        private async Task<Token> GetTokenAsync(string fileName)
        {
            var file = await _githubService.GetFileContentAsync(REPOSITORY_ID, FOLDER_PATH, fileName);

            Token? token = JsonSerializer.Deserialize<Token>(file.Content, _jsonOptions);

            if (token is null)
            {
                throw new NullReferenceException("Token is Null");
            }

            return token;
        }
    }
}
