using System;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.Meetups
{
    record CreateMeetupEventInput(string TItle, string Description, DateTime StartDateTime, long MeetupPhotoId, Uri LiveStreaming, string MeetupToken);
}
