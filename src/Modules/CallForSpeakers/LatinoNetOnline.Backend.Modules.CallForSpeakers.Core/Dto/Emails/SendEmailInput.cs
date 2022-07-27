using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Emails
{
    record SendEmailInput(string Subject, IEnumerable<Email> Emails, string Message, string HtmlMessage);
}
