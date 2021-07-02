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
    }

    class TokenRefresherManager : ITokenRefresherManager
    {
        private readonly IGitHubService _githubService;

        const long REPOSITORY_ID = 382245667;

        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };


        public TokenRefresherManager(IGitHubService githubService)
        {
            _githubService = githubService;
        }

        public async Task<Token> GetMeetupTokenAsync()
        {

            string path = "refresh-tokens";
            string fileName = "meetup.json";

            var file = await _githubService.GetFileContentAsync(REPOSITORY_ID, path, fileName);


            Token? token = JsonSerializer.Deserialize<Token>(file.Content, _jsonOptions);

            if (token is null)
                throw new NullReferenceException("Meetup Token is Null");

            return token;
        }
    }
}
