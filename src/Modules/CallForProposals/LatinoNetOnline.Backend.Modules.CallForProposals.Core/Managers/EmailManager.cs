﻿using CSharpFunctionalExtensions;

using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Emails;

using Mailjet.Client;
using Mailjet.Client.Resources;

using Microsoft.Extensions.Configuration;

using Newtonsoft.Json.Linq;

using System;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Managers
{
    interface IEmailManager
    {
        Task<Result> SendEmailAsync(SendEmailInput input);
    }

    class EmailManager : IEmailManager
    {
        private readonly IConfiguration _configuration;

        public EmailManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Result> SendEmailAsync(SendEmailInput input)
        {
            MailjetClient client = new MailjetClient("7a15b8d00d9ae904df4d492a89d03f60", _configuration["MailjetOptions:ClientSecret"]);
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
               .Property(Send.FromEmail, "callforspeaker@latinonet.online")
               .Property(Send.FromName, "Call For Speaker | Latino .NET Online")
               .Property(Send.Subject, input.Subject)
               .Property(Send.TextPart, input.Message)
               .Property(Send.HtmlPart, input.HtmlMessage)

               .Property(Send.Recipients, new JArray {
                new JObject {
                 {
                        "Email", input.ToEmail}
                 },
                 new JObject {
                 {
                        "Email", "latinonetonline@outlook.com"}
                 }
                   });
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
