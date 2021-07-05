
using System;
using System.Text.Json.Serialization;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.Meetups
{
    partial record MeetupEvent
    {
        public MeetupEvent()
        {
        }

        public MeetupEvent(string? id, string? title, string? localTime, string? localDate, Uri? link, string? description, Uri? howToFindUs, MeetupPhoto? photo)
        {
            Id = id;
            Title = title;
            LocalTime = localTime;
            LocalDate = localDate;
            Link = link;
            Description = description;
            HowToFindUs = howToFindUs;
            Photo = photo;
        }

        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Title { get; set; }

        [JsonPropertyName("local_time")]
        public string? LocalTime { get; set; }

        [JsonPropertyName("local_date")]
        public string? LocalDate { get; set; }

        public Uri? Link { get; set; }

        [JsonPropertyName("plain_text_description")]
        public string? Description { get; set; }

        [JsonPropertyName("how_to_find_us")]
        public Uri? HowToFindUs { get; set; }

        [JsonPropertyName("featured_photo")]
        public MeetupPhoto? Photo { get; set; }


        //Graph Properties

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
