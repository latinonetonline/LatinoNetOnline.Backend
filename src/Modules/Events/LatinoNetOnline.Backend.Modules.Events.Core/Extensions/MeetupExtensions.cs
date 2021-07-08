using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Meetups;
using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Webinars;

using System.Globalization;
using System.Linq;
using System.Text;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Extensions
{
    static class MeetupExtensions
    {
        public static long NormalizeId(this MeetupEvent @event)
        {
            if (string.IsNullOrWhiteSpace(@event.Id))
                return 0;

            if (long.TryParse(@event.Id.Replace("!chp", string.Empty), out long id))
            {
                return id;
            }
            else
            {
                return 0;
            }
        }

        public static string GetDescription(this WebinarDto webinar, ProposalFullDto proposal)
        {
            StringBuilder description = new();

            CultureInfo culture = new("es-ES");
            
            description.AppendLine($"Webinar Nro {webinar.Number} de la comunidad Latino .NET Online realizado el {culture.DateTimeFormat.GetDayName(webinar.StartDateTime.DayOfWeek)} {webinar.StartDateTime.Day} de {culture.DateTimeFormat.GetMonthName(webinar.StartDateTime.Month)} del {webinar.StartDateTime.Year}");
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
