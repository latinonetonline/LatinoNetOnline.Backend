using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Emails
{
    record SendEmailInput(string Subject, IEnumerable<string> Emails, string Message, string HtmlMessage);
}
