using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Meetups;
using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Webinars;
using LatinoNetOnline.Backend.Modules.Events.Core.Entities;
using LatinoNetOnline.Backend.Modules.Events.Core.Services;
using LatinoNetOnline.Backend.Modules.Events.Tests.Services;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Tests.Services
{
    public class WebinarServiceTests
    {
        #region GetAllAsync

        [Fact]
        public async Task GetAllAsync_HasntSpeakers_ResultSuccess()
        {
            MockObject mockObject = new();

            WebinarService service = mockObject.GetWebinarService();

            OperationResult<IEnumerable<WebinarDto>> result = await service.GetAllAsync();

            Assert.True(result.IsSuccess);
            Assert.False(result.Result.Any());
        }


        [Fact]
        public async Task GetAllAsync_HasWebinars_ResultSuccess()
        {
            MockObject mockObject = new();

            Uri uri = new("https://tests.com");


            mockObject.WebinarDbContext.Webinars.Add(
                new(Guid.NewGuid(), "Title", "Description", 1,  DateTime.Now, uri, uri)
                );

            mockObject.WebinarDbContext.SaveChanges();

            WebinarService service = mockObject.GetWebinarService();

            OperationResult<IEnumerable<WebinarDto>> result = await service.GetAllAsync();

            Assert.True(result.IsSuccess);
            Assert.True(result.Result.Any());
        }


        #endregion


        #region GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_WithValidId_ResultSuccess()
        {
            MockObject mockObject = new();

            Uri uri = new("https://tests.com");

            Webinar webinar = new(Guid.NewGuid(), "Title", "Description", 1, DateTime.Now, uri, uri); ;
            mockObject.WebinarDbContext.Webinars.Add(webinar);

            mockObject.WebinarDbContext.SaveChanges();

            WebinarService service = mockObject.GetWebinarService();

            var result = await service.GetByIdAsync(webinar.Id);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ResultError()
        {
            MockObject mockObject = new();

            WebinarService service = mockObject.GetWebinarService();

            var result = await service.GetByIdAsync(Guid.NewGuid());

            Assert.False(result.IsSuccess);
            Assert.Null(result.Result);

        }

        #endregion

        #region CreateAsync

        [Fact]
        public async Task CreateAsync_ResultSuccess()
        {
            MockObject mockObject = new();

            Uri uri = new("https://tests.com");

            mockObject.MeetupService.Setup(x => x.GetMeetupAsync(It.IsAny<long>())).ReturnsAsync(OperationResult<MeetupEvent>.Success(new(default, default, default, default, default, default, default, new(default, uri.ToString(), uri.ToString(), uri.ToString()))));


            mockObject.ProposalService.Setup(x => x.GetByIdAsync(It.IsAny<GetProposalInput>())).ReturnsAsync(OperationResult<ProposalFullDto>.Success(new(default, default)));


            WebinarService service = mockObject.GetWebinarService();

            OperationResult<WebinarDto> result = await service.CreateAsync(new(Guid.NewGuid(), "Title", "Description", 1, DateTime.Now));

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task CreateAsync_WithInvalidProposalId_ResultError()
        {
            MockObject mockObject = new();

            mockObject.MeetupService.Setup(x => x.GetMeetupAsync(It.IsAny<long>())).ReturnsAsync(OperationResult<MeetupEvent>.Success(new(default, default, default, default, default, default, default, default)));

            mockObject.ProposalService.Setup(x => x.GetByIdAsync(It.IsAny<GetProposalInput>())).ReturnsAsync(OperationResult<ProposalFullDto>.Fail(new("error")));

            WebinarService service = mockObject.GetWebinarService();

            OperationResult<WebinarDto> result = await service.CreateAsync(new(Guid.NewGuid(), "Title", "Description", 1, DateTime.Now));

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CreateAsync_WithMeetupIdIsZero_ResultError()
        {
            MockObject mockObject = new();

            mockObject.MeetupService.Setup(x => x.GetMeetupAsync(It.IsAny<long>())).ReturnsAsync(OperationResult<MeetupEvent>.Success(new(default, default, default, default, default, default, default, default)));

            mockObject.ProposalService.Setup(x => x.GetByIdAsync(It.IsAny<GetProposalInput>())).ReturnsAsync(OperationResult<ProposalFullDto>.Success(new(default, default)));

            WebinarService service = mockObject.GetWebinarService();

            OperationResult<WebinarDto> result = await service.CreateAsync(new(Guid.NewGuid(), "Title", "Description", 0, DateTime.Now));

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CreateAsync_WithMeetupIdInvalid_ResultError()
        {
            MockObject mockObject = new();

            mockObject.MeetupService.Setup(x => x.GetMeetupAsync(It.IsAny<long>())).ReturnsAsync(OperationResult<MeetupEvent>.Fail(new("error")));

            mockObject.ProposalService.Setup(x => x.GetByIdAsync(It.IsAny<GetProposalInput>())).ReturnsAsync(OperationResult<ProposalFullDto>.Success(new(default, default)));

            WebinarService service = mockObject.GetWebinarService();

            OperationResult<WebinarDto> result = await service.CreateAsync(new(Guid.NewGuid(),"Title", "Description", 0, DateTime.Now));

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CreateAsync_WithTitleEmply_ResultError()
        {
            MockObject mockObject = new();

            mockObject.MeetupService.Setup(x => x.GetMeetupAsync(It.IsAny<long>())).ReturnsAsync(OperationResult<MeetupEvent>.Fail(new("error")));

            mockObject.ProposalService.Setup(x => x.GetByIdAsync(It.IsAny<GetProposalInput>())).ReturnsAsync(OperationResult<ProposalFullDto>.Success(new(default, default)));

            WebinarService service = mockObject.GetWebinarService();

            OperationResult<WebinarDto> result = await service.CreateAsync(new(Guid.NewGuid(), string.Empty, "Description", 0, DateTime.Now));

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CreateAsync_WithDescriptionEmply_ResultError()
        {
            MockObject mockObject = new();

            mockObject.MeetupService.Setup(x => x.GetMeetupAsync(It.IsAny<long>())).ReturnsAsync(OperationResult<MeetupEvent>.Fail(new("error")));

            mockObject.ProposalService.Setup(x => x.GetByIdAsync(It.IsAny<GetProposalInput>())).ReturnsAsync(OperationResult<ProposalFullDto>.Success(new(default, default)));

            WebinarService service = mockObject.GetWebinarService();

            OperationResult<WebinarDto> result = await service.CreateAsync(new(Guid.NewGuid(), "Title", string.Empty, 0, DateTime.Now));

            Assert.False(result.IsSuccess);
        }


        #endregion

        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_WithValidId_ReturnSuccess()
        {
            MockObject mockObject = new();

            Uri uri = new("https://tests.com");

            Webinar webinar = new(Guid.NewGuid(), "Title", "Description", 1, DateTime.Now, uri, uri);
            mockObject.WebinarDbContext.Webinars.Add(webinar);

            mockObject.WebinarDbContext.SaveChanges();

            WebinarService service = mockObject.GetWebinarService();

            OperationResult result = await service.DeleteAsync(webinar.Id);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task DeleteAsync_WithInalidId_ReturnError()
        {
            MockObject mockObject = new();

            WebinarService service = mockObject.GetWebinarService();

            OperationResult result = await service.DeleteAsync(Guid.NewGuid());

            Assert.False(result.IsSuccess);
        }

        #endregion-


        #region GetNextWebinarAsync

        [Fact]
        public async Task GetNextWebinarAsync_HasntWebinars_ResultError()
        {
            MockObject mockObject = new();

            WebinarService service = mockObject.GetWebinarService();

            OperationResult<WebinarDto> result = await service.GetNextWebinarAsync();

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task GetNextWebinarAsync_HasOneWebinar_ResultSuccess()
        {
            MockObject mockObject = new();

            Uri uri = new("https://tests.com");

            Webinar webinar = new(Guid.NewGuid(), "Title", "Description", 1, DateTime.Now.AddDays(1), uri, uri);
            mockObject.WebinarDbContext.Webinars.Add(webinar);

            mockObject.WebinarDbContext.SaveChanges();


            WebinarService service = mockObject.GetWebinarService();

            OperationResult<WebinarDto> result = await service.GetNextWebinarAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(webinar.Id, result.Result.Id);
        }

        [Fact]
        public async Task GetNextWebinarAsync_HasTwoWebinar_ResultSuccess()
        {
            MockObject mockObject = new();

            Uri uri = new("https://tests.com");


            Webinar webinar = new(Guid.NewGuid(), "Title", "Description", 1, DateTime.Now.AddDays(1), uri, uri);
            Webinar webinar2 = new(Guid.NewGuid(), "Title", "Description", 2, DateTime.Now.AddDays(2), uri, uri);

            mockObject.WebinarDbContext.Webinars.Add(webinar);
            mockObject.WebinarDbContext.Webinars.Add(webinar2);


            mockObject.WebinarDbContext.SaveChanges();


            WebinarService service = mockObject.GetWebinarService();

            OperationResult<WebinarDto> result = await service.GetNextWebinarAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(webinar.Id, result.Result.Id);
        }

        [Fact]
        public async Task GetNextWebinarAsync_HasThreeWebinar_ResultSuccess()
        {
            MockObject mockObject = new();

            Uri uri = new("https://tests.com");

            Webinar webinar = new(Guid.NewGuid(), "Title", "Description", 1, DateTime.Now.AddDays(3), uri, uri);
            Webinar webinar2 = new(Guid.NewGuid(), "Title", "Description", 2, DateTime.Now.AddDays(2), uri, uri);
            Webinar webinar3 = new(Guid.NewGuid(), "Title", "Description", 3, DateTime.Now.AddDays(1), uri, uri);
            mockObject.WebinarDbContext.Webinars.Add(webinar2);
            mockObject.WebinarDbContext.Webinars.Add(webinar3);
            mockObject.WebinarDbContext.Webinars.Add(webinar);

            mockObject.WebinarDbContext.SaveChanges();


            WebinarService service = mockObject.GetWebinarService();

            OperationResult<WebinarDto> result = await service.GetNextWebinarAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(webinar3.Id, result.Result.Id);
        }

        #endregion
    }
}
