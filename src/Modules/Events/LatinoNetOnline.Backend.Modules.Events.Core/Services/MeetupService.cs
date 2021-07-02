using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Meetups;
using LatinoNetOnline.Backend.Modules.Events.Core.Managers;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Services
{
    interface IMeetupService
    {
        Task<OperationResult<MeetupEvent>> GetMeetupAsync(long meetupId);
        Task<OperationResult<IEnumerable<MeetupEvent>?>> GetEventsAsync();
        Task<OperationResult<MeetupPhoto>> UploadPhotoAsync(long meetupId, Stream stream);
        Task<OperationResult<MeetupEvent>> CreateEventAsync(CreateMeetupEventInput input);
    }

    class MeetupService : IMeetupService
    {
        private readonly IGraphQLManager _graphQLManager;
        private readonly ITokenRefresherManager _tokenRefresherManager;

        private readonly HttpClient _httpClient;
        const string URLNAME = "latino-net-online";
        private readonly Uri _endpoint = new("https://api.meetup.com/gql");

        public MeetupService(ITokenRefresherManager tokenRefresherManager, IGraphQLManager graphQLManager, HttpClient httpClient)
        {
            _tokenRefresherManager = tokenRefresherManager;
            _graphQLManager = graphQLManager;
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

        public async Task<OperationResult<MeetupEvent>> CreateEventAsync(CreateMeetupEventInput input)
        {
            var token = await _tokenRefresherManager.GetMeetupTokenAsync();

            string mutation = @"
                         mutation($input: CreateEventDraftInput!) {
                              createEventDraft(input: $input) {
                                event {
                                  id
                                  title
                                  eventUrl
                                  description
                                  dateTime
                                  howToFindUs
                                }
                                errors {
                                  message
                                  code
                                  field
                                }
                              }
                            }";

            var variables = new
            {
                Input = new
                {
                    GroupUrlname = "latino-net-online",
                    Title = input.TItle,
                    Description = input.Description,
                    StartDateTime = input.StartDateTime.AddHours(10).ToString("yyyy-MM-ddTHH:mm:ss"), //"2021-08-28T10:00:00",
                    VenueId = "online",
                    Duration = "PT2H",
                }
            };

            var meetupEvent = await _graphQLManager.ExceuteMutationAsync<MeetupEvent>(_endpoint, "event", mutation, variables, token.AccessToken);

            return OperationResult<MeetupEvent>.Success(meetupEvent);
        }

        public async Task<OperationResult<MeetupPhoto>> UploadPhotoAsync(long meetupId, Stream stream)
        {
            var content = new StreamContent(stream);
            var mpcontent = new MultipartFormDataContent();
            content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            mpcontent.Add(content);
            var response = await _httpClient.PostAsync($"/{URLNAME}/events/{meetupId}/photos?fields=base_url,id,self,comment_count", mpcontent);

            if (response.IsSuccessStatusCode)
            {
                var photo = await response.Content.ReadFromJsonAsync<MeetupPhoto>();

                if (photo is not null)
                    return OperationResult<MeetupPhoto>.Success(photo);
            }

            return OperationResult<MeetupPhoto>.Fail(new("error_meetup_upload_photo"));
        }
    }
}
