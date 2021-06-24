using CSharpFunctionalExtensions;

using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Emails;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Speakers;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Entities;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Services;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Tests.Services
{
    public class ProposalServiceTests
    {
        #region GetAllAsync

        [Fact]
        public async Task GetAllAsync_ResultSuccess()
        {
            MockObject mockObject = new();

            ProposalService service = mockObject.GetProposalService();

            var result = await service.GetAllAsync(new());

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetAllAsync_FilterByTitle_ResultTwoProposals()
        {
            MockObject mockObject = new();
            string title = "proposal";

            mockObject.ApplicationDbContext.Proposals.Add(
                new(title, "tests", string.Empty, string.Empty, string.Empty, new(2022, 12, 25))
                );

            mockObject.ApplicationDbContext.Proposals.Add(
                new(title, "tests", string.Empty, string.Empty, string.Empty, new(2022, 12, 25))
                );

            mockObject.ApplicationDbContext.Proposals.Add(
                new("another title", "tests", string.Empty, string.Empty, string.Empty, new(2022, 12, 25))
                );

            mockObject.ApplicationDbContext.SaveChanges();

            ProposalService service = mockObject.GetProposalService();

            var result = await service.GetAllAsync(new()
            {
                Title = title
            });

            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Result.Count());
        }


        [Fact]
        public async Task GetAllAsync_FilterByDate_ResultTwoProposals()
        {
            MockObject mockObject = new();

            mockObject.ApplicationDbContext.Proposals.Add(
                new("tests", "tests", string.Empty, string.Empty, string.Empty, new(2022, 12, 25))
                );

            mockObject.ApplicationDbContext.Proposals.Add(
                new("tests", "tests", string.Empty, string.Empty, string.Empty, new(2022, 12, 25))
                );

            mockObject.ApplicationDbContext.Proposals.Add(
                new("tests", "tests", string.Empty, string.Empty, string.Empty, new(2100, 01, 01))
                );

            mockObject.ApplicationDbContext.SaveChanges();

            ProposalService service = mockObject.GetProposalService();

            var result = await service.GetAllAsync(new()
            {
                Date = new(2022, 12, 25)
            });

            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Result.Count());
        }

        [Fact]
        public async Task GetAllAsync_FilterByIsActive_ResultTwoProposals()
        {
            MockObject mockObject = new();

            mockObject.ApplicationDbContext.Proposals.Add(
                new("tests", "tests", string.Empty, string.Empty, string.Empty, true, new(2100, 01, 01), DateTime.Now, new List<Speaker>())
                );

            mockObject.ApplicationDbContext.Proposals.Add(
                new("tests", "tests", string.Empty, string.Empty, string.Empty, false, new(2100, 01, 01), DateTime.Now, new List<Speaker>())
                );

            mockObject.ApplicationDbContext.Proposals.Add(
                new("tests", "tests", string.Empty, string.Empty, string.Empty, true, new(2100, 01, 01), DateTime.Now, new List<Speaker>())
                );

            mockObject.ApplicationDbContext.SaveChanges();

            ProposalService service = mockObject.GetProposalService();

            var result = await service.GetAllAsync(new()
            {
                IsActive = true
            });

            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Result.Count());
        }


        #endregion

        #region GetAllDatesAsync

        [Fact]
        public async Task GetAllDatesAsync_ResultSuccessWithoutDates()
        {
            MockObject mockObject = new();

            ProposalService service = mockObject.GetProposalService();

            var result = await service.GetAllDatesAsync();

            Assert.True(result.IsSuccess);
            Assert.Empty(result.Result.Dates);
        }


        [Fact]
        public async Task GetAllDatesAsync_ResultSuccessWithDate()
        {
            MockObject mockObject = new();

            DateTime eventDate = new(2022, 12, 25);

            mockObject.ApplicationDbContext.Proposals.Add(
                new("test", "tests", string.Empty, string.Empty, string.Empty, eventDate)
                );

            mockObject.ApplicationDbContext.SaveChanges();

            ProposalService service = mockObject.GetProposalService();

            var result = await service.GetAllDatesAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(eventDate, result.Result.Dates.Single());
        }

        #endregion

        #region GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_WithValidId_ResultSuccess()
        {
            MockObject mockObject = new();

            Proposal proposal = new("test", "tests", string.Empty, string.Empty, string.Empty, new(2022, 12, 25));

            mockObject.ApplicationDbContext.Proposals.Add(proposal);

            mockObject.ApplicationDbContext.SaveChanges();

            ProposalService service = mockObject.GetProposalService();

            var result = await service.GetByIdAsync(proposal.Id);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ResultError()
        {
            MockObject mockObject = new();

            ProposalService service = mockObject.GetProposalService();

            var result = await service.GetByIdAsync(Guid.NewGuid());

            Assert.False(result.IsSuccess);
        }

        #endregion

        #region CreateAsync

        [Fact]
        public async Task CreateAsync_ResultSuccess()
        {
            MockObject mockObject = new();

            mockObject.EmailManagerMock.Setup(s => s.SendEmailAsync(It.IsAny<SendEmailInput>())).ReturnsAsync(Result.Success());

            CreateSpeakerInput speakerInput = GetCreateSpeakerInput();

            ProposalService service = mockObject.GetProposalService();



            var result = await service.CreateAsync(
                new("test", "tests", new(2046, 08, 18), "test", "test", "test", new List<CreateSpeakerInput>() { speakerInput }));

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }


        [Fact]
        public async Task CreateAsync_WithoutSpeakers_ResultError()
        {
            MockObject mockObject = new();

            mockObject.EmailManagerMock.Setup(s => s.SendEmailAsync(It.IsAny<SendEmailInput>())).ReturnsAsync(Result.Success());

            ProposalService service = mockObject.GetProposalService();

            var result = await service.CreateAsync(
                new("test", "tests", new(2046, 08, 18), "test", "test", "test", new List<CreateSpeakerInput>()));

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CreateAsync_WithEmplyTitle_ResultError()
        {
            MockObject mockObject = new();

            CreateSpeakerInput speakerInput = GetCreateSpeakerInput();

            ProposalService service = mockObject.GetProposalService();

            OperationResult<ProposalFullDto> result = await service.CreateAsync(
                new(string.Empty, "tests", new(2046, 08, 18), "test", "test", "test", new List<CreateSpeakerInput>() { speakerInput }));

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CreateAsync_WithEmplyDescription_ResultError()
        {
            MockObject mockObject = new();

            CreateSpeakerInput speakerInput = GetCreateSpeakerInput();

            ProposalService service = mockObject.GetProposalService();

            OperationResult<ProposalFullDto> result = await service.CreateAsync(
                new("test", string.Empty, new(2046, 08, 18), "test", "test", "test", new List<CreateSpeakerInput>() { speakerInput }));

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CreateAsync_WithEventDateLessThanToday_ResultError()
        {
            MockObject mockObject = new();

            CreateSpeakerInput speakerInput = GetCreateSpeakerInput();

            ProposalService service = mockObject.GetProposalService();

            OperationResult<ProposalFullDto> result = await service.CreateAsync(
                new("test", "tests", new(2020, 08, 18), "test", "test", "test", new List<CreateSpeakerInput>() { speakerInput }));

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CreateAsync_WithEventDateIsNotSaturday_ResultError()
        {
            MockObject mockObject = new();

            CreateSpeakerInput speakerInput = GetCreateSpeakerInput();

            ProposalService service = mockObject.GetProposalService();

            OperationResult<ProposalFullDto> result = await service.CreateAsync(
                new("test", "tests", new(2046, 08, 19), "test", "test", "test", new List<CreateSpeakerInput>() { speakerInput }));

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CreateAsync_WithEventDateAlreadyTaken_ResultError()
        {
            MockObject mockObject = new();

            DateTime eventDate = new(2046, 08, 18);

            mockObject.ApplicationDbContext.Add(new Proposal("test", "tests", string.Empty, string.Empty, string.Empty, eventDate));

            mockObject.ApplicationDbContext.SaveChanges();

            CreateSpeakerInput speakerInput = GetCreateSpeakerInput();

            ProposalService service = mockObject.GetProposalService();

            OperationResult<ProposalFullDto> result = await service.CreateAsync(
                new("test", "tests", eventDate, "test", "test", "test", new List<CreateSpeakerInput>() { speakerInput }));

            Assert.False(result.IsSuccess);
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_WithValidId_ReturnSuccess()
        {
            MockObject mockObject = new();

            Proposal proposal = new("test", "tests", string.Empty, string.Empty, string.Empty, new(2046, 08, 18));

            mockObject.ApplicationDbContext.Add(proposal);

            mockObject.ApplicationDbContext.SaveChanges();

            ProposalService service = mockObject.GetProposalService();

            OperationResult result = await service.DeleteAsync(proposal.Id);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task DeleteAsync_WithInalidId_ReturnError()
        {
            MockObject mockObject = new();

            ProposalService service = mockObject.GetProposalService();

            OperationResult result = await service.DeleteAsync(Guid.NewGuid());

            Assert.False(result.IsSuccess);
        }

        #endregion


        #region DeleteAllAsync

        [Fact]
        public async Task DeleteAllAsync_WithExistProposals_ReturnSuccess()
        {
            MockObject mockObject = new();

            Proposal proposal = new("test", "tests", string.Empty, string.Empty, string.Empty, new(2046, 08, 18));

            EntityEntry<Proposal> entry = mockObject.ApplicationDbContext.Add(proposal);

            await mockObject.ApplicationDbContext.SaveChangesAsync();

            entry.State = EntityState.Detached;

            ProposalService service = mockObject.GetProposalService();

            OperationResult result = await service.DeleteAllAsync();

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task DeleteAllAsync_WithoutExistProposals_ReturnSuccess()
        {
            MockObject mockObject = new();

            ProposalService service = mockObject.GetProposalService();

            OperationResult result = await service.DeleteAllAsync();

            Assert.True(result.IsSuccess);
        }

        #endregion

        private static CreateSpeakerInput GetCreateSpeakerInput()
            => new("test", "test", "test@tests.com", "test", "test", new("https://tests.com"));
    }
}
