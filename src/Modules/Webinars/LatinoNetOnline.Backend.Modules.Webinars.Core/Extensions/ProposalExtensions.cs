using AngleSharp;

using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Emails;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Webinars;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Extensions
{
    static class ProposalExtensions
    {
        public static ProposalDto ConvertToDto(this Proposal proposal)
            => new(proposal.Id, proposal.Title, proposal.Description, proposal.EventDate, proposal.CreationTime, proposal.AudienceAnswer, proposal.KnowledgeAnswer, proposal.UseCaseAnswer, proposal.IsActive, proposal.WebinarNumber, proposal.Status, proposal.Meetup, proposal.Streamyard, proposal.LiveStreaming, proposal.Flyer);


        public static async Task<SendEmailInput> ConvertToProposalCreatedEmailInput(this ProposalFullDto proposal)
        {
            StringBuilder message = new();
            message.AppendLine($"Charla: { proposal.Proposal.Title}");
            message.AppendLine($"Fecha: { proposal.Proposal.EventDate:dd/MM/yyyy}");



            return new SendEmailInput($"Recibimos su postulación del Call For Speaker de Latino .NET Online",
          proposal.Speakers.Select(x => new Email(x.Email)),
          message.ToString(),
          await BuildProposalCreatedEmail(proposal));
        }

        public static async Task<SendEmailInput> ConvertToProposalConfirmedEmailInput(this ProposalFullDto proposal)
        {
            StringBuilder message = new();
            message.AppendLine($"Charla: { proposal.Proposal.Title}");
            message.AppendLine($"Fecha: { proposal.Proposal.EventDate:dd/MM/yyyy}");



            return new SendEmailInput($"Confirmación del Call For Speaker de Latino .NET Online",
          proposal.Speakers.Select(x => new Email(x.Email)),
          message.ToString(),
          await BuildProposalConfirmedEmail(proposal));
        }

        private static async Task<string> BuildProposalConfirmedEmail(ProposalFullDto proposal)
        {
            CultureInfo culture = new("es-Es");
            TextInfo textInfo = culture.TextInfo;

            var html = File.ReadAllText(Path.Combine(new FileInfo(typeof(ProposalExtensions).Assembly.Location).DirectoryName ?? string.Empty, "Files/mail-proposal-confirmed.html"));

            var config = Configuration.Default;

            //Create a new context for evaluating webpages with the given config
            var context = BrowsingContext.New(config);

            //Create a virtual request to specify the document to load (here from our fixed string)
            var document = await context.OpenAsync(req => req.Content(html));

            var firstSpeaker = proposal.Speakers.First();

            string webinarDate = proposal.Proposal.EventDate.ToString("dddd dd 'de' MMMM 'del' yyyy", culture);

            document.GetElementById("webinar-flyer").SetAttribute("src", proposal.Proposal.Flyer!.ToString());
            document.GetElementById("proposal-title").TextContent = proposal.Proposal.Title;
            document.GetElementById("proposal-description").TextContent = proposal.Proposal.Description;

            document.GetElementById("streamyard-link-1").SetAttribute("href", proposal.Proposal.Streamyard!.ToString());
            document.GetElementById("streamyard-link-2").SetAttribute("href", proposal.Proposal.Streamyard!.ToString());
            document.GetElementById("streamyard-link-2").TextContent = ShortLink(proposal.Proposal.Streamyard);


            document.GetElementById("meetup-link-1").SetAttribute("href", proposal.Proposal.Meetup!.ToString());
            document.GetElementById("meetup-link-2").SetAttribute("href", proposal.Proposal.Meetup!.ToString());
            document.GetElementById("meetup-link-2").TextContent = ShortLink(proposal.Proposal.Meetup);

            static string ShortLink(Uri link)
                => link.ToString().Replace("https://", string.Empty).Replace("www.", string.Empty);




            var newHtml = document.DocumentElement.OuterHtml;


            return newHtml;
        }

        private static async Task<string> BuildProposalCreatedEmail(ProposalFullDto proposal)
        {
            CultureInfo culture = new("es-Es");
            TextInfo textInfo = culture.TextInfo;

            var html = File.ReadAllText(Path.Combine(new FileInfo(typeof(ProposalExtensions).Assembly.Location).DirectoryName ?? string.Empty, "Files/mail-proposal-created.html"));

            var config = Configuration.Default;

            //Create a new context for evaluating webpages with the given config
            var context = BrowsingContext.New(config);

            //Create a virtual request to specify the document to load (here from our fixed string)
            var document = await context.OpenAsync(req => req.Content(html));

            var firstSpeaker = proposal.Speakers.First();

            string webinarDate = proposal.Proposal.EventDate.ToString("dddd dd 'de' MMMM 'del' yyyy", culture);

            document.GetElementById("proposal-title").TextContent = proposal.Proposal.Title;
            document.GetElementById("proposal-description").TextContent = proposal.Proposal.Description;
            document.GetElementById("proposal-date").TextContent = textInfo.ToTitleCase(webinarDate);

            document.GetElementById("speaker-name").TextContent = $"{firstSpeaker.Name} {firstSpeaker.LastName}";
            document.GetElementById("speaker-description").TextContent = firstSpeaker.Description;

            document.GetElementById("speaker-image").SetAttribute("src", firstSpeaker.Image.ToString());

            if (proposal.Speakers.Count() == 1)
            {
                document.GetElementById("second-speaker").Remove();
            }
            else
            {
                var secondSpeaker = proposal.Speakers.First(s => s != firstSpeaker);

                document.GetElementById("second-speaker-name").TextContent = $"{secondSpeaker.Name} {secondSpeaker.LastName}";
                document.GetElementById("second-speaker-description").TextContent = secondSpeaker.Description;

                document.GetElementById("second-speaker-image").SetAttribute("src", secondSpeaker.Image.ToString());
            }



            var newHtml = document.DocumentElement.OuterHtml;


            return newHtml;
        }


        public static Task<int?> MaxNumberAsync(this IQueryable<Proposal> query)
            => query.AsNoTracking().MaxAsync(x => (int?)x.WebinarNumber);

        public static void UpdateWebinarNumber(this IEnumerable<Proposal> webinars, int lastWebinarConfirmated)
        {
            var webinarsList = webinars.OrderBy(x => x.EventDate).ToList();

            for (int i = 0; i < webinarsList.Count; i++)
            {
                var webinar = webinarsList[i];

                int nextWebinarNumber = lastWebinarConfirmated + i + 1;

                webinar.WebinarNumber = nextWebinarNumber;
            }
        }

        public static string GetDescription(this ProposalFullDto proposal)
        {
            StringBuilder description = new();

            CultureInfo culture = new("es-ES");

            description.AppendLine($"Webinar Nro {proposal.Proposal.WebinarNumber} de la comunidad Latino .NET Online realizado el {culture.DateTimeFormat.GetDayName(proposal.Proposal.EventDate.DayOfWeek)} {proposal.Proposal.EventDate.Day} de {culture.DateTimeFormat.GetMonthName(proposal.Proposal.EventDate.Month)} del {proposal.Proposal.EventDate.Year}");
            description.AppendLine();
            description.Append($"🎤Speakers: ");


            var speaker = proposal.Speakers.ElementAt(0);

            description.AppendFormat("{0} {1}", speaker.Name, speaker.LastName);

            if (proposal.Speakers.Count() == 2)
            {
                speaker = proposal.Speakers.ElementAt(1);

                description.AppendFormat(" y {0} {1}", speaker.Name, speaker.LastName);
            }


            description.AppendLine();
            description.AppendLine();
            description.AppendLine($"📚Tema: {proposal.Proposal.Description}");
            description.AppendLine();
            description.AppendLine("Suscríbete! :)");
            description.AppendLine();
            description.AppendLine("Like y comparte! :)");
            description.AppendLine();

            description.AppendLine("📌 Nuestras Redes Sociales: 📌");

            description.AppendLine("🔴 Sitio Web: https://latinonet.online​​​​​​​​​​");

            description.AppendLine("🔴 Twitter: https://twitter.com/LatinoNETOnline​​");

            description.AppendLine("🔴 Servidor de Discord: https://go.latinonet.online/discord");

            description.AppendLine("🔴 Grupo de Telegram: https://go.latinonet.online/telegram");

            return description.ToString();

        }
    }
}
