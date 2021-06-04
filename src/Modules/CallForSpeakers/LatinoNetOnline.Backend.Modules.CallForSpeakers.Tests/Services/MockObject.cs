using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Data;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Managers;

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
        }

        public ApplicationDbContext ApplicationDbContext { get; set; }
        public Mock<IEmailManager> EmailManagerMock { get; set; }
    }
}
