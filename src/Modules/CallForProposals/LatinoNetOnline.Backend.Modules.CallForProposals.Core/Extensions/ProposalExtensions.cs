using AngleSharp;

using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Emails;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Entities;

using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Extensions
{
    static class ProposalExtensions
    {
        public static ProposalDto ConvertToDto(this Proposal proposal)
            => new(proposal.Id, proposal.Title, proposal.Description, proposal.EventDate, proposal.CreationTime, proposal.AudienceAnswer, proposal.KnowledgeAnswer, proposal.UseCaseAnswer);


        public static async Task<SendEmailInput> ConvertToEmailInput(this ProposalFullDto proposal)
        {
            StringBuilder message = new();
            message.AppendLine($"Charla: { proposal.Proposal.Title}");
            message.AppendLine($"Fecha: { proposal.Proposal.EventDate:dd/MM/yyyy}");



            return new SendEmailInput($"Confirmación del Call For Speaker de Latino .NET Online",
          proposal.Speakers.Select(x => x.Email),
          message.ToString(),
          await BuildEmail(proposal));
        }

        private static async Task<string> BuildEmail(ProposalFullDto proposal)
        {
            CultureInfo culture = new("es-Es");
            TextInfo textInfo = culture.TextInfo;

            var html = File.ReadAllText(Path.Combine(new FileInfo(typeof(ProposalExtensions).Assembly.Location).DirectoryName ?? string.Empty, "Files/mail.html"));

            var config = Configuration.Default;

            //Create a new context for evaluating webpages with the given config
            var context = BrowsingContext.New(config);

            //Create a virtual request to specify the document to load (here from our fixed string)
            var document = await context.OpenAsync(req => req.Content(html));

            var firstSpeaker = proposal.Speakers.First();

            string webinarDate = proposal.Proposal.EventDate.ToString("dddd dd 'de' MMMM 'del' yyyy", culture);

            document.GetElementById("propusal-title").TextContent = proposal.Proposal.Title;
            document.GetElementById("propusal-description").TextContent = proposal.Proposal.Description;
            document.GetElementById("propusal-date").TextContent = textInfo.ToTitleCase(webinarDate);

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
    }
}
