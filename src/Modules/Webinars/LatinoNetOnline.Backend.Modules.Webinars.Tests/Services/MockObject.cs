﻿using LatinoNetOnline.Backend.Modules.Webinars.Core.Data;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Managers;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Services;
using LatinoNetOnline.Backend.Shared.Abstractions.Messaging;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using Moq;

using System;

namespace LatinoNetOnline.Backend.Modules.Webinars.Tests.Services
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
            StorageServiceMock = new();
            HttpContextAccessorMock = new();
        }

        public ApplicationDbContext ApplicationDbContext { get; set; }
        public Mock<IEmailManager> EmailManagerMock { get; set; }
        public Mock<IMessageBroker> MessageBrokerMock { get; set; }
        public Mock<IStorageService> StorageServiceMock { get; set; }
        public Mock<IHttpContextAccessor> HttpContextAccessorMock { get; set; }


        public ProposalService GetProposalService()
            => new(ApplicationDbContext, EmailManagerMock.Object, MessageBrokerMock.Object, StorageServiceMock.Object, HttpContextAccessorMock.Object);

    }
}
