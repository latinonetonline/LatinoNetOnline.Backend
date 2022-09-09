using CSharpFunctionalExtensions;

using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Emails;

using Mailjet.Client;
using Mailjet.Client.Resources;

using Microsoft.Extensions.Configuration;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Managers
{
    class EmailManager : IEmailManager
    {
        private readonly IConfiguration _configuration;

        public EmailManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Result> SendEmailAsync(SendEmailInput input)
        {
            MailjetClient client = new("7a15b8d00d9ae904df4d492a89d03f60", _configuration["MailjetOptions:ClientSecret"]);

            List<JObject> emails = new();


            foreach (var email in input.Emails)
            {
                emails.Add(new JObject {

                             { "Email", email.ToString()}
                         });
            }

            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
               .Property(Send.FromEmail, "callforspeaker@latinonet.online")
               .Property(Send.FromName, "Call For Speaker | Latino .NET Online")
               .Property(Send.Subject, input.Subject)
               .Property(Send.TextPart, input.Message)
               .Property(Send.HtmlPart, input.HtmlMessage)
               .Property(Send.Recipients, new JArray(emails.ToArray()));


            MailjetResponse response = await client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
                Console.WriteLine(response.GetData());
                return Result.Success();
            }
            else
            {
                Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
                Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
                Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));

                return Result.Failure(response.GetErrorMessage());
            }

        }
    }
}
