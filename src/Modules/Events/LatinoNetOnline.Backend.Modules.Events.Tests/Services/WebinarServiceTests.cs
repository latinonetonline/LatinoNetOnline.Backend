using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Webinars;
using LatinoNetOnline.Backend.Modules.Events.Core.Entities;
using LatinoNetOnline.Backend.Modules.Events.Core.Enums;
using LatinoNetOnline.Backend.Modules.Events.Core.Services;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

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

            Uri uri = new("https://tests.com");
            Webinar webinar = new(Guid.NewGuid(), 1, 1, uri, uri, uri);
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

            Uri uri = new("https://tests.com");

            Webinar webinar = new(Guid.NewGuid(), 1, 1, uri, uri, uri);
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

            Uri uri = new("https://tests.com");


            Webinar webinar = new(Guid.NewGuid(), 1, 1, uri, uri, uri);
            Webinar webinar2 = new(Guid.NewGuid(), 2, 2, uri, uri, uri);

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

            Uri uri = new("https://tests.com");

            Webinar webinar = new(Guid.NewGuid(), 1, 1, uri, uri, uri);
            Webinar webinar2 = new(Guid.NewGuid(), 2, 2, uri, uri, uri);
            Webinar webinar3 = new(Guid.NewGuid(), 3, 3, uri, uri, uri);
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
    }
}
