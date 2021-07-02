using System;
using System.Text.Json.Serialization;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.Meetups
{
    record MeetupPhoto
    {
        public MeetupPhoto()
        {
        }

        public MeetupPhoto(long id, Uri? highresLink, Uri? photoLink, Uri? thumbLink)
        {
            Id = id;
            HighresLink = highresLink;
            PhotoLink = photoLink;
            ThumbLink = thumbLink;
        }

        public long Id { get; set; }

        [JsonPropertyName("highres_link")]
        public Uri? HighresLink { get; set; }

        [JsonPropertyName("photo_link")]
        public Uri? PhotoLink { get; set; }

        [JsonPropertyName("thumb_link")]
        public Uri? ThumbLink { get; set; }
    }
}
