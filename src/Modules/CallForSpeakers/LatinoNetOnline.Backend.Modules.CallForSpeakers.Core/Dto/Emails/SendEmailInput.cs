using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Emails
{
    record SendEmailInput(string Subject, IEnumerable<string> Emails, string Message, string HtmlMessage);
}
