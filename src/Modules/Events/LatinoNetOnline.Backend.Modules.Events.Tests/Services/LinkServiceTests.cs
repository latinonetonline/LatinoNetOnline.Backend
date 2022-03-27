using LatinoNetOnline.Backend.Modules.Events.Core.Dto.GitHub;
using LatinoNetOnline.Backend.Modules.Events.Core.Entities;
using LatinoNetOnline.Backend.Modules.Events.Core.Options;
using LatinoNetOnline.Backend.Modules.Events.Core.Services;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace LatinoNetOnline.Backend.Modules.Events.Tests.Services
{
    public class LinkServiceTests
    {
        #region GetAllAsync

        [Fact]
        public async Task GetAllAsync_HasntLinks_ResultSuccess()
        {
            MockObject mockObject = new();

            mockObject.GithubService.Setup(x => x.GetAllFilesWithContentAsync(It.IsAny<long>(), It.IsAny<string>())).ReturnsAsync(Enumerable.Empty<GhFileContent>());

            mockObject.GithubOptions.Setup(x => x.Value).Returns(new GithubOptions());

            LinkService service = mockObject.GetLinkService();

            var result = await service.GetAllAsync();

            Assert.True(result.IsSuccess);
            Assert.False(result.Result.Any());
        }


        [Fact]
        public async Task GetAllAsync_HasLinks_ResultSuccess()
        {
            MockObject mockObject = new();

            string jsonLink =
                            @"{
                              ""Name"": ""sitioweb"",
                              ""Url"": ""https://latinonet.online""
                            }
                            ";

            mockObject.GithubService.Setup(x => x.GetAllFilesWithContentAsync(It.IsAny<long>(), It.IsAny<string>())).ReturnsAsync(new GhFileContent[] { new("", jsonLink, "") });

            mockObject.GithubOptions.Setup(x => x.Value).Returns(new GithubOptions());

            LinkService service = mockObject.GetLinkService();

            var result = await service.GetAllAsync();

            Assert.True(result.IsSuccess);
            Assert.True(result.Result.Any());
            Assert.NotEmpty(result.Result.First().Name);
            Assert.NotEmpty(result.Result.First().Url);
        }


        #endregion


        #region GetByNameAsync

        [Fact]
        public async Task GetByNameAsync_WithValidName_ResultSuccess()
        {
            MockObject mockObject = new();

            string jsonLink =
                            @"{
                              ""Name"": ""sitioweb"",
                              ""Url"": ""https://latinonet.online""
                            }";

            mockObject.GithubService.Setup(x => x.GetFileContentAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new GhFileContent("", jsonLink, ""));

            mockObject.GithubOptions.Setup(x => x.Value).Returns(new GithubOptions());

            var service = mockObject.GetLinkService();

            var result = await service.GetAsync("sitioweb");

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task GetByNameAsync_WithInvalidName_ResultError()
        {
            MockObject mockObject = new();

   

            mockObject.GithubService.Setup(x => x.GetFileContentAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null);

            mockObject.GithubOptions.Setup(x => x.Value).Returns(new GithubOptions());

            var service = mockObject.GetLinkService();

            var result = await service.GetAsync("sitioweb");

            Assert.False(result.IsSuccess);

        }

        #endregion


        #region CreateAsync

        [Fact]
        public async Task CreateAsync_ResultSuccess()
        {
            MockObject mockObject = new();

            mockObject.GithubService.Setup(x => x.CreateFileAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new GhFileContent("", "", ""));

            mockObject.GithubOptions.Setup(x => x.Value).Returns(new GithubOptions());

            LinkService service = mockObject.GetLinkService();



            var result = await service.CreateAsync(
                new("sitioweb", "https://latinonet.online"));

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        #endregion


        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_ResultSuccess()
        {
            MockObject mockObject = new();

            mockObject.GithubService.Setup(x => x.UpdateFileAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            mockObject.GithubOptions.Setup(x => x.Value).Returns(new GithubOptions());

            LinkService service = mockObject.GetLinkService();



            var result = await service.UpdateAsync(
                new("sitioweb", "https://latinonet.online"));

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        #endregion


        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_ResultSuccess()
        {
            MockObject mockObject = new();

            mockObject.GithubService.Setup(x => x.DeleteFileAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>()));

            mockObject.GithubOptions.Setup(x => x.Value).Returns(new GithubOptions());

            LinkService service = mockObject.GetLinkService();



            var result = await service.DeleteAsync("sitioweb");

            Assert.True(result.IsSuccess);
        }

        #endregion

    }
}
