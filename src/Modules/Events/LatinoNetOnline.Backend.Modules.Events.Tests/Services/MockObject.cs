using LatinoNetOnline.Backend.Modules.Events.Core.Data;
using LatinoNetOnline.Backend.Modules.Events.Core.Services;

using Microsoft.EntityFrameworkCore;

using Moq;

using System;

namespace LatinoNetOnline.Backend.Modules.Events.Tests.Services
{
    class MockObject
    {
        public MockObject()
        {
            WebinarDbContext = new EventDbContext(
                    new DbContextOptionsBuilder<EventDbContext>()
                        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                        .Options
                );

            MeetupService = new();
            ProposalService = new();
        }

        public EventDbContext WebinarDbContext { get; set; }
        public Mock<IMeetupService> MeetupService { get; set; }
        public Mock<IProposalService> ProposalService { get; set; }


        public WebinarService GetWebinarService()
            => new(WebinarDbContext, MeetupService.Object, ProposalService.Object);
    }
}
