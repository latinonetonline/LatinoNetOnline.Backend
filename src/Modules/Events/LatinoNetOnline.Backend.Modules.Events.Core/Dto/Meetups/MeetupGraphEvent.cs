using System;
using System.Text.Json.Serialization;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.Meetups
{
    partial record MeetupEvent
    {
        [JsonPropertyName("title")]
        public string? TitleGraph { set { Title = value; } }

        [JsonPropertyName("dateTime")]
        public string? LocalDateGraph { set { LocalDate = value; } }
        [JsonPropertyName("eventUrl")]
        public Uri? LinkGraph { set { Link = value; } }

        [JsonPropertyName("description")]
        public string? DescriptionGraph { set { Description = value; } }

        [JsonPropertyName("howToFindUs")]
        public Uri? HowToFindUsGraph { set { HowToFindUs = value; } }
    }
}
