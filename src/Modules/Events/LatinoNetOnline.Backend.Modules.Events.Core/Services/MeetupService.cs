using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Meetups;
using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Meetups.GraphTypes;
using LatinoNetOnline.Backend.Modules.Events.Core.Managers;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
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
        Task<OperationResult<MeetupEvent>> UpdateEventAsync(UpdateMeetupEventInput input);
        Task<OperationResult<MeetupEvent>> PublishEventAsync(long meetupId);
        Task<OperationResult<MeetupEvent>> AnnounceEventAsync(long meetupId);
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
            var token = await _tokenRefresherManager.GetMeetupTokenAsync();

            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);

            var response = await _httpClient.GetAsync($"https://api.meetup.com/{URLNAME}/events/{meetupId}?fields=featured_photo,plain_text_description");

            if (!response.IsSuccessStatusCode)
                return OperationResult<MeetupEvent>.Fail(new("meetup_not_found"));

            var meetup = await response.Content.ReadFromJsonAsync<MeetupEvent>();

            if (meetup is null)
                return OperationResult<MeetupEvent>.Fail(new("meetup_not_found"));

            return OperationResult<MeetupEvent>.Success(meetup);
        }

        public async Task<OperationResult<IEnumerable<MeetupEvent>?>> GetEventsAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<MeetupEvent>>($"https://api.meetup.com/{URLNAME}/events?fields=featured_photo,plain_text_description");

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

            var meetupEvent = await _graphQLManager.ExceuteMutationAsync<CreateEventDraftResponse>(_endpoint, "createEventDraft", mutation, variables, token.AccessToken);

            return OperationResult<MeetupEvent>.Success(meetupEvent.Event);
        }


        public async Task<OperationResult<MeetupEvent>> UpdateEventAsync(UpdateMeetupEventInput input)
        {
            var token = await _tokenRefresherManager.GetMeetupTokenAsync();

            string mutation = @"
                         mutation($input: EditEventInput!) {
                              editEvent(input: $input) {
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
                    EventId = input.EventId,
                    Title = input.Title,
                    Description = input.Description,
                    StartDateTime = input.StartDateTime.AddHours(10).ToString("yyyy-MM-ddTHH:mm:ss"), //"2021-08-28T10:00:00",
                    HowToFindUs = input.HowToFindUs,
                    FeaturedPhotoId = input.PhotoId,
                    VenueId = "online",
                    Duration = "PT2H",
                }
            };

            var meetupEvent = await _graphQLManager.ExceuteMutationAsync<CreateEventDraftResponse>(_endpoint, "editEvent", mutation, variables, token.AccessToken);

            return OperationResult<MeetupEvent>.Success(meetupEvent.Event);
        }

        public async Task<OperationResult<MeetupPhoto>> UploadPhotoAsync(long meetupId, Stream stream)
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StreamContent(stream), "photo", "photo");

            var request = new HttpRequestMessage(HttpMethod.Post, $"https://api.meetup.com/{URLNAME}/events/{meetupId}/photos?fields=base_url,id,self,comment_count")
            {
                Content = formData
            };

            var token = await _tokenRefresherManager.GetMeetupTokenAsync();

            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var photo = await response.Content.ReadFromJsonAsync<MeetupPhoto>();

                if (photo is not null)
                    return OperationResult<MeetupPhoto>.Success(photo);
            }

            var stringresponse = await response.Content.ReadAsStringAsync();

            return OperationResult<MeetupPhoto>.Fail(new("error_meetup_upload_photo"));
        }

        public async Task<OperationResult<MeetupEvent>> PublishEventAsync(long meetupId)
        {
            var token = await _tokenRefresherManager.GetMeetupTokenAsync();

            string mutation = @"
                         mutation($input: publishEventDraftInput!) {
                              publishEventDraft(input: $input) {
                                event {
                                  id
                                  title
                                  eventUrl
                                  description
                                  dateTime
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
                    EventId = meetupId
                }
            };

            var meetupEvent = await _graphQLManager.ExceuteMutationAsync<CreateEventDraftResponse>(_endpoint, "publishEventDraft", mutation, variables, token.AccessToken);


            if (meetupEvent is null || meetupEvent.Event is null)
            {
                return OperationResult<MeetupEvent>.Fail(new("meetup_publish_error"));
            }

            return OperationResult<MeetupEvent>.Success(meetupEvent.Event);
        }

        public async Task<OperationResult<MeetupEvent>> AnnounceEventAsync(long meetupId)
        {
            var token = await _tokenRefresherManager.GetMeetupTokenAsync();

            string mutation = @"
                         mutation($input: AnnounceEventInput!) {
                              announceEvent(input: $input) {
                                event {
                                  id
                                  title
                                  eventUrl
                                  description
                                  dateTime
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
                    EventId = meetupId
                }
            };

            var meetupEvent = await _graphQLManager.ExceuteMutationAsync<CreateEventDraftResponse>(_endpoint, "announceEvent", mutation, variables, token.AccessToken);

            if (meetupEvent is null || meetupEvent.Event is null)
            {
                return OperationResult<MeetupEvent>.Fail(new("meetup_announce_error"));
            }

            return OperationResult<MeetupEvent>.Success(meetupEvent.Event);
        }
    }
}
