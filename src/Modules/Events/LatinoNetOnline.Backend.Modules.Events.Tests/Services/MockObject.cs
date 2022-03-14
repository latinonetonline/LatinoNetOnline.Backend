using LatinoNetOnline.Backend.Modules.Events.Core.Data;
using LatinoNetOnline.Backend.Modules.Events.Core.Managers;
using LatinoNetOnline.Backend.Modules.Events.Core.Services;
using LatinoNetOnline.Backend.Shared.Abstractions.Events;
using LatinoNetOnline.Backend.Shared.Abstractions.Messaging;

using Microsoft.EntityFrameworkCore;

using Moq;

using System;

namespace LatinoNetOnline.Backend.Modules.Events.Tests.Services
{
    class MockObject
    {
        public MockObject()
        {
            ApplicationDbContext = new ApplicationDbContext(
                    new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                        .Options
                );

            EmailManagerMock = new();
            MessageBrokerMock = new();
            MeetupService = new();
            ProposalService = new();
            EventDispatcher = new();
            MessageBroker = new();
        }

        public ApplicationDbContext ApplicationDbContext { get; set; }
        public Mock<IEmailManager> EmailManagerMock { get; set; }
        public Mock<IMessageBroker> MessageBrokerMock { get; set; }
        public Mock<IMeetupService> MeetupService { get; set; }
        public Mock<IProposalService> ProposalService { get; set; }
        public Mock<IEventDispatcher> EventDispatcher { get; set; }
        public Mock<IMessageBroker> MessageBroker { get; set; }


        public ProposalService GetProposalService()
            => new(ApplicationDbContext, EmailManagerMock.Object, MessageBrokerMock.Object);

        public WebinarService GetWebinarService()
           => new(ApplicationDbContext, MeetupService.Object, ProposalService.Object, MessageBroker.Object);

    }
}
