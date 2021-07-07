using System;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.Meetups
{
    record UpdateMeetupEventInput(long EventId, string Title, string Description, DateTime StartDateTime, Uri? HowToFindUs, long? PhotoId);
}
