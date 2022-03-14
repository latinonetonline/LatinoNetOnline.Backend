using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.Emails
{
    record SendEmailInput(string Subject, IEnumerable<Email> Emails, string Message, string HtmlMessage);
}
