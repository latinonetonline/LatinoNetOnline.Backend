using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Meetups;
using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Meetups.GraphTypes;
using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Meetups.Objects;
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
        Task<OperationResult<ImageUploadResponse>> UploadPhotoAsync(UploadImageInput input);
        Task<OperationResult<MeetupEvent>> CreateEventAsync(CreateMeetupEventInput input);
        Task<OperationResult<MeetupEvent>> UpdateEventAsync(UpdateMeetupEventInput input);
        Task<OperationResult<MeetupEvent>> PublishEventAsync(long meetupId);
        Task<OperationResult> DeleteEventAsync(long meetupId);
        Task<OperationResult<MeetupEvent>> AnnounceEventAsync(long meetupId);
    }

    class MeetupService : IMeetupService
    {
        private readonly IGraphQLManager _graphQLManager;
        private readonly ITokenRefresherManager _tokenRefresherManager;
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly HttpClient _httpClient;
        const string URLNAME = "latino-net-online";
        const long GROUPID = 34573949;
        private readonly Uri _endpoint = new("https://api.meetup.com/gql");

        public MeetupService(ITokenRefresherManager tokenRefresherManager, IGraphQLManager graphQLManager, HttpClient httpClient, IHttpClientFactory httpClientFactory)
        {
            _tokenRefresherManager = tokenRefresherManager;
            _graphQLManager = graphQLManager;
            _httpClient = httpClient;
            _httpClientFactory = httpClientFactory;
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
                         mutation($input: CreateEventInput!) {
                              createEvent(input: $input) {
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
                    input.TItle,
                    input.Description,
                    StartDateTime = input.StartDateTime.AddHours(9).ToString("yyyy-MM-ddTHH:mm:ss"), //"2021-08-28T10:00:00",
                    VenueId = "online",
                    Duration = "PT2H",
                    PublishStatus = "DRAFT"
                }
            };

            var meetupEvent = await _graphQLManager.ExceuteMutationAsync<CreateEventDraftResponse>(_endpoint, "createEvent", mutation, variables, token.AccessToken);

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
                    input.EventId,
                    input.Title,
                    input.Description,
                    StartDateTime = input.StartDateTime.AddHours(10).ToString("yyyy-MM-ddTHH:mm:ss"), //"2021-08-28T10:00:00",
                    input.HowToFindUs,
                    FeaturedPhotoId = input.PhotoId,
                    VenueId = "online",
                    Duration = "PT2H",
                }
            };

            var meetupEvent = await _graphQLManager.ExceuteMutationAsync<CreateEventDraftResponse>(_endpoint, "editEvent", mutation, variables, token.AccessToken);

            return OperationResult<MeetupEvent>.Success(meetupEvent.Event);
        }

        public async Task<OperationResult<MeetupPhoto>> UploadPhotoOldAsync(long meetupId, Stream stream)
        {
            var formData = new MultipartFormDataContent
            {
                { new StreamContent(stream), "photo", "photo" }
            };

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

            return OperationResult<MeetupPhoto>.Fail(new("error_meetup_upload_photo"));
        }

        public async Task<OperationResult<ImageUploadResponse>> UploadPhotoAsync(UploadImageInput input)
        {
            var token = await _tokenRefresherManager.GetMeetupTokenAsync();

            string mutation = @"
                         mutation($input: ImageUploadInput!) {
                              uploadImage(input: $input) {
                                uploadUrl
                                image {
                                  id
                                  baseUrl
                                  preview
                                }
                                imagePath
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
                    GroupId = GROUPID,
                    input.PhotoType,
                    input.FileName,
                    input.ContentType
                }
            };

            var imageUploadResponse = await _graphQLManager.ExceuteMutationAsync<ImageUploadResponse>(_endpoint, "uploadImage", mutation, variables, token.AccessToken);


            HttpClient httpClient = _httpClientFactory.CreateClient();


            StreamContent requestContent = new(input.Stream);

            requestContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

            requestContent.Headers.ContentLength = input.Stream.Length;


            HttpResponseMessage result = await httpClient.PutAsync(imageUploadResponse.UploadUrl, requestContent);

            if (!result.IsSuccessStatusCode)
            {
                return OperationResult<ImageUploadResponse>.Fail(new("Hubo un error al subir la imagen al Bucket de S3"));
            }

            return OperationResult<ImageUploadResponse>.Success(imageUploadResponse);
        }

        public async Task<OperationResult<MeetupEvent>> PublishEventAsync(long meetupId)
        {
            var token = await _tokenRefresherManager.GetMeetupTokenAsync();

            string mutation = @"
                         mutation($input: PublishEventDraftInput!) {
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

        public async Task<OperationResult> DeleteEventAsync(long meetupId)
        {
            var token = await _tokenRefresherManager.GetMeetupTokenAsync();

            string mutation = @"
                         mutation($input: DeleteEventInput!) {
                              deleteEvent(input: $input) {
                                success

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
                    EventId = meetupId,
                    RemoveFromCalendar = true
                }
            };

            await _graphQLManager.ExceuteMutationAsync<DeleteEventResponse>(_endpoint, "deleteEvent", mutation, variables, token.AccessToken);

            return OperationResult.Success();
        }
    }
}
