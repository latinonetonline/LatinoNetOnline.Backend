using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Website.GetNextWebinars
{
    public record GetNextWebinarsResponse(string Title, string Description, Uri Flyer, Uri LiveStreaming, Uri Meetup, DateTime EventDate);
}