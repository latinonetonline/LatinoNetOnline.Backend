using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Webinars;
using LatinoNetOnline.Backend.Modules.Events.Core.Entities;
using LatinoNetOnline.Backend.Modules.Events.Core.Enums;
using LatinoNetOnline.Backend.Modules.Events.Core.Services;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;

namespace LatinoNetOnline.Backend.Modules.Events.Tests.Services
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

            Proposal proposal = new("test", "tests", string.Empty, string.Empty, string.Empty, new(2022, 12, 25));

            mockObject.ApplicationDbContext.Proposals.Add(proposal);

            Uri uri = new("https://tests.com");
            Webinar webinar = new(proposal.Id, 1, 1, uri, uri, uri);
            webinar.Status = WebinarStatus.Draft;
            mockObject.ApplicationDbContext.Webinars.Add(webinar);

            mockObject.ApplicationDbContext.SaveChanges();

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

            Webinar webinar = new(Guid.NewGuid(), 1, 1, uri, uri, uri); ;
            mockObject.ApplicationDbContext.Webinars.Add(webinar);

            mockObject.ApplicationDbContext.SaveChanges();

            WebinarService service = mockObject.GetWebinarService();

            var result = await service.GetByIdAsync(new(webinar.Id));

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ResultError()
        {
            MockObject mockObject = new();

            WebinarService service = mockObject.GetWebinarService();

            var result = await service.GetByIdAsync(new(Guid.NewGuid()));

            Assert.False(result.IsSuccess);
            Assert.Null(result.Result);

        }

        #endregion


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

            Proposal proposal = new("test", "tests", string.Empty, string.Empty, string.Empty, new(2022, 12, 25));

            mockObject.ApplicationDbContext.Proposals.Add(proposal);

            mockObject.ApplicationDbContext.SaveChanges();

            Uri uri = new("https://tests.com");

            Webinar webinar = new(proposal.Id, 1, 1, uri, uri, uri);
            mockObject.ApplicationDbContext.Webinars.Add(webinar);

            mockObject.ApplicationDbContext.SaveChanges();


            WebinarService service = mockObject.GetWebinarService();

            OperationResult<WebinarDto> result = await service.GetNextWebinarAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(webinar.Id, result.Result.Id);
        }

        [Fact]
        public async Task GetNextWebinarAsync_HasTwoWebinar_ResultSuccess()
        {
            MockObject mockObject = new();

            Proposal proposal1 = new("test", "tests", string.Empty, string.Empty, string.Empty, DateTime.Now.AddDays(1));

            Proposal proposal2 = new("test", "tests", string.Empty, string.Empty, string.Empty, DateTime.Now.AddDays(2));

            mockObject.ApplicationDbContext.Proposals.Add(proposal1);
            mockObject.ApplicationDbContext.Proposals.Add(proposal2);

            Uri uri = new("https://tests.com");


            Webinar webinar = new(proposal1.Id, 1, 1, uri, uri, uri);
            Webinar webinar2 = new(proposal2.Id, 2, 2, uri, uri, uri);

            mockObject.ApplicationDbContext.Webinars.Add(webinar);
            mockObject.ApplicationDbContext.Webinars.Add(webinar2);


            mockObject.ApplicationDbContext.SaveChanges();


            WebinarService service = mockObject.GetWebinarService();

            OperationResult<WebinarDto> result = await service.GetNextWebinarAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(webinar.Id, result.Result.Id);
        }

        [Fact]
        public async Task GetNextWebinarAsync_HasThreeWebinar_ResultSuccess()
        {
            MockObject mockObject = new();

            Proposal proposal1 = new("test", "tests", string.Empty, string.Empty, string.Empty, DateTime.Now.AddDays(3));

            Proposal proposal2 = new("test", "tests", string.Empty, string.Empty, string.Empty, DateTime.Now.AddDays(2));

            Proposal proposal3 = new("test", "tests", string.Empty, string.Empty, string.Empty, DateTime.Now.AddDays(1));

            mockObject.ApplicationDbContext.Proposals.Add(proposal1);
            mockObject.ApplicationDbContext.Proposals.Add(proposal2);
            mockObject.ApplicationDbContext.Proposals.Add(proposal3);

            Uri uri = new("https://tests.com");

            Webinar webinar = new(proposal1.Id, 1, 1, uri, uri, uri);
            Webinar webinar2 = new(proposal2.Id, 2, 2, uri, uri, uri);
            Webinar webinar3 = new(proposal3.Id, 3, 3, uri, uri, uri);
            mockObject.ApplicationDbContext.Webinars.Add(webinar2);
            mockObject.ApplicationDbContext.Webinars.Add(webinar3);
            mockObject.ApplicationDbContext.Webinars.Add(webinar);

            mockObject.ApplicationDbContext.SaveChanges();


            WebinarService service = mockObject.GetWebinarService();

            OperationResult<WebinarDto> result = await service.GetNextWebinarAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(webinar3.Id, result.Result.Id);
        }

        #endregion


        #region UpdateWebinarNumbersAsync

        [Fact]
        public async Task UpdateWebinarNumbersAsync_HasntWebinars_ResultError()
        {
            MockObject mockObject = new();


            WebinarService service = mockObject.GetWebinarService();

            var result = await service.UpdateWebinarNumbersAsync();

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateWebinarNumbersAsync_HasWebinars_ResultSuccess()
        {
            MockObject mockObject = new();

            Proposal proposal1 = new("test", "tests", string.Empty, string.Empty, string.Empty, DateTime.Today.AddDays(1));

            Proposal proposal2 = new("test", "tests", string.Empty, string.Empty, string.Empty, DateTime.Today.AddDays(2));

            Proposal proposal3 = new("test", "tests", string.Empty, string.Empty, string.Empty, DateTime.Today.AddDays(3));

            mockObject.ApplicationDbContext.Proposals.Add(proposal1);
            mockObject.ApplicationDbContext.Proposals.Add(proposal2);
            mockObject.ApplicationDbContext.Proposals.Add(proposal3);

            Uri uri = new("https://tests.com");


            Webinar webinar = new(proposal1.Id, 0, 1, uri, uri, uri);
            Webinar webinar2 = new(proposal2.Id, 0, 2, uri, uri, uri);
            Webinar webinar3 = new(proposal3.Id, 0, 2, uri, uri, uri);

            mockObject.ApplicationDbContext.Webinars.Add(webinar);
            mockObject.ApplicationDbContext.Webinars.Add(webinar2);
            mockObject.ApplicationDbContext.Webinars.Add(webinar3);


            mockObject.ApplicationDbContext.SaveChanges();

            WebinarService service = mockObject.GetWebinarService();

            var result = await service.UpdateWebinarNumbersAsync();

            webinar = mockObject.ApplicationDbContext.Webinars.First(x => x.Id == webinar.Id);
            webinar2 = mockObject.ApplicationDbContext.Webinars.First(x => x.Id == webinar2.Id);
            webinar3 = mockObject.ApplicationDbContext.Webinars.First(x => x.Id == webinar3.Id);




            Assert.True(result.IsSuccess);
            Assert.Equal(1, webinar.Number);
            Assert.Equal(2, webinar2.Number);
            Assert.Equal(3, webinar3.Number);
        }

        [Fact]
        public async Task UpdateWebinarNumbersAsync_HasPastWebinarPublished_ResultSuccess()
        {
            MockObject mockObject = new();

            Proposal proposal1 = new("test", "tests", string.Empty, string.Empty, string.Empty, DateTime.Today.AddDays(-1));

            Proposal proposal2 = new("test", "tests", string.Empty, string.Empty, string.Empty, DateTime.Today.AddDays(2));

            Proposal proposal3 = new("test", "tests", string.Empty, string.Empty, string.Empty, DateTime.Today.AddDays(3));

            mockObject.ApplicationDbContext.Proposals.Add(proposal1);
            mockObject.ApplicationDbContext.Proposals.Add(proposal2);
            mockObject.ApplicationDbContext.Proposals.Add(proposal3);

            Uri uri = new("https://tests.com");


            Webinar webinar = new(proposal1.Id, 4, 1, uri, uri, uri);
            webinar.Status = WebinarStatus.Published;

            Webinar webinar2 = new(proposal2.Id, 0, 2, uri, uri, uri);
            Webinar webinar3 = new(proposal3.Id, 0, 2, uri, uri, uri);

            mockObject.ApplicationDbContext.Webinars.Add(webinar);
            mockObject.ApplicationDbContext.Webinars.Add(webinar2);
            mockObject.ApplicationDbContext.Webinars.Add(webinar3);


            mockObject.ApplicationDbContext.SaveChanges();

            WebinarService service = mockObject.GetWebinarService();

            var result = await service.UpdateWebinarNumbersAsync();

            webinar = mockObject.ApplicationDbContext.Webinars.First(x => x.Id == webinar.Id);
            webinar2 = mockObject.ApplicationDbContext.Webinars.First(x => x.Id == webinar2.Id);
            webinar3 = mockObject.ApplicationDbContext.Webinars.First(x => x.Id == webinar3.Id);




            Assert.True(result.IsSuccess);
            Assert.Equal(4, webinar.Number);
            Assert.Equal(5, webinar2.Number);
            Assert.Equal(6, webinar3.Number);
        }

       

        #endregion

    }
}
