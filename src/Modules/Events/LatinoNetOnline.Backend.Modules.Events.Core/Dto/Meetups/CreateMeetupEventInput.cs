using System;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.Meetups
{
    record CreateMeetupEventInput(string Title, string Description, DateTime StartDateTime);
}
