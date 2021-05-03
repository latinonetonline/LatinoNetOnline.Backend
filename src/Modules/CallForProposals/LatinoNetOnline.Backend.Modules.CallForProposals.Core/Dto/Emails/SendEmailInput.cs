namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Emails
{
    record SendEmailInput(string Subject, string ToEmail, string Message, string HtmlMessage);
}
