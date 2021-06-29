using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Meetups;
using LatinoNetOnline.Backend.Modules.Events.Core.Managers;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Services
{
    interface IMeetupService
    {
        Task<OperationResult<MeetupEvent>> GetMeetupAsync(long meetupId);
        Task<OperationResult<IEnumerable<MeetupEvent>?>> GetEventsAsync();
    }

    class MeetupService : IMeetupService
    {
        private readonly IGraphQLManager _graphQLManager;
        private readonly HttpClient _httpClient;
        const string URLNAME = "latino-net-online";
        private readonly Uri _endpoint = new ("https://api.meetup.com/gql");

        public MeetupService(IGraphQLManager graphQLManager, HttpClient httpClient)
        {
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

            string mutation = @"
                         mutation($input: CreateEventDraftInput!) {
                              createEventDraft(input: $input) {
                                event {
                                  id
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
                    StartDateTime = input.StartDateTime.ToString(), //"2021-08-28T10:00:00",
                    VenueId = "online",
                    Duration = "PT2H",
                    HowToFindUs = input.LiveStreaming.ToString(),
                    FeaturedPhotoId = input.MeetupPhotoId

                }
            };

            var meetupEvent = await _graphQLManager.ExceuteMutationAsync<MeetupEvent>(_endpoint, "event", mutation, variables, input.MeetupToken);

            return OperationResult<MeetupEvent>.Success(meetupEvent);
        }
    }
}
