using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Data;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Metricool;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Extensions;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.EntityFrameworkCore;

using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Services
{
    interface IMetricoolService
    {
        Task<OperationResult<MetricoolExportDto>> ExportSocialFileAsync(MetricoolExportInput input);
    }

    class MetricoolService : IMetricoolService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IProposalService _proposalService;

        public MetricoolService(ApplicationDbContext dbContext, IProposalService proposalService)
        {
            _dbContext = dbContext;
            _proposalService = proposalService;
        }

        public async Task<OperationResult<MetricoolExportDto>> ExportSocialFileAsync(MetricoolExportInput input)
        {
            var proposal = await _dbContext.Proposals.AsNoTracking().Include(x => x.Speakers).SingleAsync(x => x.Id == input.ProposalId);
            ProposalFullDto proposalFullDto = new(proposal.ConvertToDto(), proposal.Speakers.Select(x => x.ConvertToDto()));


            StringBuilder metricoolText = new();
            metricoolText.AppendLine(GetFileHeader());



            AddWebinarAlertPosts(metricoolText, proposalFullDto);

            AddWebinarDescriptionPosts(metricoolText, proposalFullDto);

            AddEnUnaHoraComenzamosPosts(metricoolText, proposalFullDto);

            AddEstamosOnlinePosts(metricoolText, proposalFullDto);


            return OperationResult<MetricoolExportDto>.Success(new(metricoolText.ToString(), $"Webinar Nro {proposal.WebinarNumber} Metricool File.csv"));
        }


        static void AddEnUnaHoraComenzamosPosts(StringBuilder metricoolText, ProposalFullDto proposal)
        {
            string enUnaHoraComenzamosText = string.Format(GetEnUnaHoraComenzamosTemplate(), proposal.Proposal.LiveStreaming);

            metricoolText.AppendLine($"\"{enUnaHoraComenzamosText}\";\"{proposal.Proposal.EventDate:yyyy-MM-dd}\";\"10:45\";true;false;false;false;false;false;;\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";false;;");

            metricoolText.AppendLine($"\"{enUnaHoraComenzamosText}\";\"{proposal.Proposal.EventDate:yyyy-MM-dd}\";\"10:45\";false;true;false;false;false;false;;\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";false;;");
        }

        static void AddEstamosOnlinePosts(StringBuilder metricoolText, ProposalFullDto proposal)
        {
            var speaker = proposal.Speakers.ElementAt(0);

            StringBuilder speakerText = new($"{speaker.Name} {speaker.LastName} ({speaker.Twitter})");

            if (proposal.Speakers.Count() == 2)
            {
                speaker = proposal.Speakers.ElementAt(1);

                speakerText.AppendFormat(" y {0} {1} ({3})", speaker.Name, speaker.LastName, speaker.Twitter);
            }


            string estamosOnlineText = string.Format(GetEstamosOnlineTemplate(), speakerText, proposal.Proposal.LiveStreaming);

            metricoolText.AppendLine($"\"{estamosOnlineText}\";\"{proposal.Proposal.EventDate:yyyy-MM-dd}\";\"12:10\";true;false;false;false;false;false;;\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";false;;");

            metricoolText.AppendLine($"\"{estamosOnlineText}\";\"{proposal.Proposal.EventDate:yyyy-MM-dd}\";\"12:10\";false;true;false;false;false;false;;\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";false;;");
        }

        static void AddWebinarDescriptionPosts(StringBuilder metricoolText, ProposalFullDto proposal)
        {
            string descriptionText = string.Format(GetDescriptionTemplate(), proposal.Proposal.Description, proposal.Proposal.Meetup);

            metricoolText.AppendLine($"\"{descriptionText}\";\"{proposal.Proposal.EventDate.AddDays(-1):yyyy-MM-dd}\";\"15:00\";true;false;false;false;false;false;;\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";false;;");

            metricoolText.AppendLine($"\"{descriptionText}\";\"{proposal.Proposal.EventDate.AddDays(-1):yyyy-MM-dd}\";\"15:00\";false;true;false;false;false;false;;\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";false;;");
        }


        static void AddWebinarAlertPosts(StringBuilder metricoolText, ProposalFullDto proposal)
        {
            var speaker = proposal.Speakers.ElementAt(0);
            StringBuilder speakerText = new(speaker.Twitter);

            if (proposal.Speakers.Count() == 2)
            {
                speaker = proposal.Speakers.ElementAt(1);

                speakerText.AppendFormat(" y {0}", speaker.Twitter);
            }

            string webinarAlertTwitterText = string.Format(GetWebinarAlertTemplate(), GetDateString(proposal.Proposal.EventDate), proposal.Proposal.Title, speakerText.ToString(), proposal.Proposal.Meetup);


            speaker = proposal.Speakers.ElementAt(0);
            speakerText = new($"{speaker.Name} {speaker.LastName} ({speaker.Twitter})");

            if (proposal.Speakers.Count() == 2)
            {
                speaker = proposal.Speakers.ElementAt(1);

                speakerText.AppendFormat(" y {0} {1} ({3})", speaker.Name, speaker.LastName, speaker.Twitter);
            }

            string webinarAlertFacebookText = string.Format(GetWebinarAlertTemplate(), GetDateString(proposal.Proposal.EventDate), proposal.Proposal.Title, speakerText.ToString(), proposal.Proposal.Meetup);

            metricoolText.AppendLine($"\"{webinarAlertFacebookText}\";\"{proposal.Proposal.EventDate.AddDays(-2):yyyy-MM-dd}\";\"15:00\";true;false;false;false;false;false;;\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";false;;");

            metricoolText.AppendLine($"\"{webinarAlertTwitterText}\";\"{proposal.Proposal.EventDate.AddDays(-2):yyyy-MM-dd}\";\"15:00\";false;true;false;false;false;false;;\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";false;;");
        }


        static string GetFileHeader()
            => "Text;Date;Time;Facebook;Twitter;LinkedIn;GMB;Instagram;Pinterest;Picture Url 1;Picture Url 2;Picture Url 3;Picture Url 4;Picture Url 5;Picture Url 6;Picture Url 7;Picture Url 8;Picture Url 9;Picture Url 10;Shortener;Pinterest Board;Pinterest Pin Title";

        static string GetWebinarAlertTemplate()
            => @"🚨 Webinar Alert 🚨

🕙 Cuando: {0} a las 15:00 UTC

📚 Tema: {1}

🎤 Speaker: {2}
 
🔗 Meetup link: {3}

#dotnet 

Los esperamos! 😉";


        static string GetEnUnaHoraComenzamosTemplate()
            => @"En una hora arrancamos 👇👇👇

{0}

#dotnet  #webinar";



        static string GetEstamosOnlineTemplate()
            => @"Estamos Online 🔥🔥
Junto a 🎤 {0}

{1}

#dotnet #webinar ";

        static string GetDescriptionTemplate()
            => @"{0}

{1}

Los Esperamos 😉

#dotnet #webinar";


        static string GetDateString(DateTime dateTime)
        {
            CultureInfo culture = new("es-ES");
            return $"{culture.TextInfo.ToTitleCase(culture.DateTimeFormat.GetDayName(dateTime.DayOfWeek))} {dateTime.Day} de {culture.TextInfo.ToTitleCase(culture.DateTimeFormat.GetMonthName(dateTime.Month))} del {dateTime.Year}";
        }


    }
}
