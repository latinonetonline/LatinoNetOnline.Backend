using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Data;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Managers;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Services;
using LatinoNetOnline.Backend.Shared.Abstractions.Events;
using LatinoNetOnline.Backend.Shared.Abstractions.Messaging;

using Microsoft.EntityFrameworkCore;

using Moq;

using System;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Tests.Services
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
            EventDispatcherMock = new();
        }

        public ApplicationDbContext ApplicationDbContext { get; set; }
        public Mock<IEmailManager> EmailManagerMock { get; set; }
        public Mock<IMessageBroker> EventDispatcherMock { get; set; }


        public ProposalService GetService()
            => new(ApplicationDbContext, EmailManagerMock.Object, EventDispatcherMock.Object);
    }
}
