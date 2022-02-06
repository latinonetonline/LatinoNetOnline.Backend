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

        const string URLNAME = "latino-net-online";
        const long GROUPID = 34573949;
        private readonly Uri _endpoint = new("https://api.meetup.com/gql");

        public MeetupService(ITokenRefresherManager tokenRefresherManager, IGraphQLManager graphQLManager, IHttpClientFactory httpClientFactory)
        {
            _tokenRefresherManager = tokenRefresherManager;
            _graphQLManager = graphQLManager;
            _httpClientFactory = httpClientFactory;
        }


        public async Task<OperationResult<MeetupEvent>> GetMeetupAsync(long meetupId)
        {
            var token = await _tokenRefresherManager.GetMeetupTokenAsync();

            string query = @"
                         query {
                              event(id: "+ meetupId  + @") {
                           
                                  id
                                  title
                                  eventUrl
                                  description
                                  dateTime
                                  howToFindUs
                                  image{
                                    id
                                  }

                              }
                            }";


            var meetupEvent = await _graphQLManager.ExceuteQueryAsync<MeetupEvent>(_endpoint, "event", query, null, token.AccessToken);

            return OperationResult<MeetupEvent>.Success(meetupEvent);
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
                    input.Title,
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
