using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Meetups;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Services
{
    interface IMeetupService
    {
        Task<OperationResult<MeetupEvent>> GetMeetupAsync(long meetupId);
        Task<OperationResult<IEnumerable<MeetupEvent>?>> GetEventsAsync();
    }

    class MeetupService : IMeetupService
    {
        private readonly HttpClient _httpClient;
        const string URLNAME = "latino-net-online";

        public MeetupService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OperationResult<MeetupEvent>> GetMeetupAsync(long meetupId)
        {
            var response = await _httpClient.GetAsync($"{URLNAME}/events/{meetupId}?fields=featured_photo,plain_text_description");

            if (!response.IsSuccessStatusCode)
                return OperationResult<MeetupEvent>.Fail(new("meetup_not_found"));

            var meetup = await response.Content.ReadFromJsonAsync<MeetupEvent>();

            if (meetup is null)
                return OperationResult<MeetupEvent>.Fail(new("meetup_not_found"));

            return OperationResult<MeetupEvent>.Success(meetup);
        }

        public async Task<OperationResult<IEnumerable<MeetupEvent>?>> GetEventsAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<MeetupEvent>>($"{URLNAME}/events?fields=featured_photo,plain_text_description");

            return OperationResult<IEnumerable<MeetupEvent>?>.Success(result);
        }
    }
}
