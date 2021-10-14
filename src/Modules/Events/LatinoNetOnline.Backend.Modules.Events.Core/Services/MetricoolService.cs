using LatinoNetOnline.Backend.Modules.Events.Core.Data;
using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Metricool;
using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.Events.Core.Entities;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.EntityFrameworkCore;

using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Services
{
    interface IMetricoolService
    {
        Task<OperationResult<MetricoolExportDto>> ExportSocialFileAsync(MetricoolExportInput input);
    }

    class MetricoolService : IMetricoolService
    {
        private readonly EventDbContext _dbContext;
        private readonly IProposalService _proposalService;

        public MetricoolService(EventDbContext dbContext, IProposalService proposalService)
        {
            _dbContext = dbContext;
            _proposalService = proposalService;
        }

        public async Task<OperationResult<MetricoolExportDto>> ExportSocialFileAsync(MetricoolExportInput input)
        {
            var webinar = await _dbContext.Webinars.AsNoTracking().SingleAsync(x => x.Id == input.WebinarId);

            var proposalResult = await _proposalService.GetByIdAsync(new(webinar.ProposalId));

            if (!proposalResult.IsSuccess)
            {
                return OperationResult<MetricoolExportDto>.Fail(proposalResult.Error);
            }

            var proposal = proposalResult.Result;

            StringBuilder metricoolText = new();
            metricoolText.AppendLine(GetFileHeader());



            AddWebinarAlertPosts(metricoolText, proposal, webinar);

            AddWebinarDescriptionPosts(metricoolText, proposal, webinar);

            AddEnUnaHoraComenzamosPosts(metricoolText, proposal, webinar);

            AddEstamosOnlinePosts(metricoolText, proposal, webinar);


            return OperationResult<MetricoolExportDto>.Success(new(metricoolText.ToString(), $"Webinar Nro {webinar.Number} Metricool File.csv"));
        }


        void AddEnUnaHoraComenzamosPosts(StringBuilder metricoolText, ProposalFullDto proposal, Webinar webinar)
        {
            string enUnaHoraComenzamosText = string.Format(GetEnUnaHoraComenzamosTemplate(), webinar.MeetupId);

            metricoolText.AppendLine($"\"{enUnaHoraComenzamosText}\";\"{proposal.Proposal.EventDate:yyyy-MM-dd}\";\"10:45\";true;false;false;false;false;false;;\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";false;;");

            metricoolText.AppendLine($"\"{enUnaHoraComenzamosText}\";\"{proposal.Proposal.EventDate:yyyy-MM-dd}\";\"10:45\";false;true;false;false;false;false;;\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";false;;");
        }

        void AddEstamosOnlinePosts(StringBuilder metricoolText, ProposalFullDto proposal, Webinar webinar)
        {
            var speaker = proposal.Speakers.ElementAt(0);

            StringBuilder speakerText = new($"{speaker.Name} {speaker.LastName} ({speaker.Twitter})");

            if (proposal.Speakers.Count() == 2)
            {
                speaker = proposal.Speakers.ElementAt(1);

                speakerText.AppendFormat(" y {0} {1} ({3})", speaker.Name, speaker.LastName, speaker.Twitter);
            }


            string estamosOnlineText = string.Format(GetEstamosOnlineTemplate(), speakerText, webinar.LiveStreaming);

            metricoolText.AppendLine($"\"{estamosOnlineText}\";\"{proposal.Proposal.EventDate:yyyy-MM-dd}\";\"12:10\";true;false;false;false;false;false;;\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";false;;");

            metricoolText.AppendLine($"\"{estamosOnlineText}\";\"{proposal.Proposal.EventDate:yyyy-MM-dd}\";\"12:10\";false;true;false;false;false;false;;\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";false;;");
        }

        void AddWebinarDescriptionPosts(StringBuilder metricoolText, ProposalFullDto proposal, Webinar webinar)
        {
            string descriptionText = string.Format(GetDescriptionTemplate(), proposal.Proposal.Description, webinar.MeetupId);

            metricoolText.AppendLine($"\"{descriptionText}\";\"{proposal.Proposal.EventDate.AddDays(-1):yyyy-MM-dd}\";\"15:00\";true;false;false;false;false;false;;\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";false;;");

            metricoolText.AppendLine($"\"{descriptionText}\";\"{proposal.Proposal.EventDate.AddDays(-1):yyyy-MM-dd}\";\"15:00\";false;true;false;false;false;false;;\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";false;;");
        }


        void AddWebinarAlertPosts(StringBuilder metricoolText, ProposalFullDto proposal, Webinar webinar)
        {
            var speaker = proposal.Speakers.ElementAt(0);
            StringBuilder speakerText = new(speaker.Twitter);

            if (proposal.Speakers.Count() == 2)
            {
                speaker = proposal.Speakers.ElementAt(1);

                speakerText.AppendFormat(" y {0}", speaker.Twitter);
            }

            string webinarAlertTwitterText = string.Format(GetWebinarAlertTemplate(), GetDateString(proposal.Proposal.EventDate), proposal.Proposal.Title, speakerText.ToString(), webinar.MeetupId);


            speaker = proposal.Speakers.ElementAt(0);
            speakerText = new($"{speaker.Name} {speaker.LastName} ({speaker.Twitter})");

            if (proposal.Speakers.Count() == 2)
            {
                speaker = proposal.Speakers.ElementAt(1);

                speakerText.AppendFormat(" y {0} {1} ({3})", speaker.Name, speaker.LastName, speaker.Twitter);
            }

            string webinarAlertFacebookText = string.Format(GetWebinarAlertTemplate(), GetDateString(proposal.Proposal.EventDate), proposal.Proposal.Title, speakerText.ToString(), webinar.MeetupId);

            metricoolText.AppendLine($"\"{webinarAlertFacebookText}\";\"{proposal.Proposal.EventDate.AddDays(-2):yyyy-MM-dd}\";\"15:00\";true;false;false;false;false;false;;\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";false;;");

            metricoolText.AppendLine($"\"{webinarAlertTwitterText}\";\"{proposal.Proposal.EventDate.AddDays(-2):yyyy-MM-dd}\";\"15:00\";false;true;false;false;false;false;;\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";false;;");
        }


        string GetFileHeader()
            => "Text;Date;Time;Facebook;Twitter;LinkedIn;GMB;Instagram;Pinterest;Picture Url 1;Picture Url 2;Picture Url 3;Picture Url 4;Picture Url 5;Picture Url 6;Picture Url 7;Picture Url 8;Picture Url 9;Picture Url 10;Shortener;Pinterest Board;Pinterest Pin Title";

        string GetWebinarAlertTemplate()
            => @"🚨 Webinar Alert 🚨

🕙 Cuando: {0} a las 15:00 UTC

📚 Tema: {1}

🎤 Speaker: {2}
 
🔗 Meetup link: https://www.meetup.com/es/latino-net-online/events/{3}/

#dotnet 

Los esperamos! 😉";


        string GetEnUnaHoraComenzamosTemplate()
            => @"En una hora arrancamos 👇👇👇

https://www.meetup.com/es/latino-net-online/events/{0}/

#dotnet  #webinar";



        string GetEstamosOnlineTemplate()
            => @"Estamos Online 🔥🔥
Junto a 🎤 {0}

{1}

#dotnet #webinar ";

        string GetDescriptionTemplate()
            => @"{0}

https://www.meetup.com/es/latino-net-online/events/{1}/

Los Esperamos 😉

#dotnet #webinar";


        string GetDateString(DateTime dateTime)
        {
            CultureInfo culture = new("es-ES");
            return $"{culture.TextInfo.ToTitleCase(culture.DateTimeFormat.GetDayName(dateTime.DayOfWeek))} {dateTime.Day} de {culture.TextInfo.ToTitleCase(culture.DateTimeFormat.GetMonthName(dateTime.Month))} del {dateTime.Year}";
        }


    }
}
