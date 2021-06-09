using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Speakers;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Services;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Tests.Services
{
    public class SpeakerServiceTests
    {

        #region CreateAsync
        [Fact]
        public async Task CreateAsync_WithEmplyName_ResultError()
        {
            MockObject mockObject = new();

            SpeakerService service = new(mockObject.ApplicationDbContext);

            CreateSpeakerInput input = new(string.Empty, "lastname", new("test@tests.com"), "@tests", "tests", new("https://tests.com"));


            OperationResult<SpeakerDto> result = await service.CreateAsync(input);

            Assert.False(result.IsSuccess);
        }


        [Fact]
        public async Task CreateAsync_WithEmplyLastName_ResultError()
        {
            MockObject mockObject = new();

            SpeakerService service = new(mockObject.ApplicationDbContext);

            CreateSpeakerInput input = new("test", string.Empty, new("test@tests.com"), "@tests", "tests", new("https://tests.com"));


            OperationResult<SpeakerDto> result = await service.CreateAsync(input);

            Assert.False(result.IsSuccess);
        }


        [Fact]
        public async Task CreateAsync_WithEmplyEmail_ResultError()
        {
            MockObject mockObject = new();

            SpeakerService service = new(mockObject.ApplicationDbContext);

            CreateSpeakerInput input = new("tests", "lastname", null, "@tests", "tests", new("https://tests.com"));


            OperationResult<SpeakerDto> result = await service.CreateAsync(input);

            Assert.False(result.IsSuccess);
        }


        [Fact]
        public async Task CreateAsync_WithInvalidEmail_ResultError()
        {
            MockObject mockObject = new();

            SpeakerService service = new(mockObject.ApplicationDbContext);

            CreateSpeakerInput input = new("tests", "lastname", new("test@tests.com"), "@tests", string.Empty, new("https://tests.com"));


            OperationResult<SpeakerDto> result = await service.CreateAsync(input);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CreateAsync_WithNullImage_ResultError()
        {
            MockObject mockObject = new();

            SpeakerService service = new(mockObject.ApplicationDbContext);

            CreateSpeakerInput input = new("tests", "lastname", new("test@tests.com"), "@tests", "tests", null);


            OperationResult<SpeakerDto> result = await service.CreateAsync(input);

            Assert.False(result.IsSuccess);
        }


        [Fact]
        public async Task CreateAsync_WithEmplyDescription_ResultError()
        {
            MockObject mockObject = new();

            SpeakerService service = new(mockObject.ApplicationDbContext);

            CreateSpeakerInput input = new("tests", "lastname", new("test@tests.com"), "@tests", string.Empty, new("https://tests.com"));


            OperationResult<SpeakerDto> result = await service.CreateAsync(input);

            Assert.False(result.IsSuccess);
        }


        [Fact]
        public async Task CreateAsync_WithEmplyTwitter_ResultSuccess()
        {
            MockObject mockObject = new();

            SpeakerService service = new(mockObject.ApplicationDbContext);

            CreateSpeakerInput input = new("tests", "lastname", new("test@tests.com"), string.Empty, "tests", new("https://tests.com"));


            OperationResult<SpeakerDto> result = await service.CreateAsync(input);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task CreateAsync_WithValidInput_ResultSuccess()
        {
            MockObject mockObject = new();

            SpeakerService service = new(mockObject.ApplicationDbContext);

            CreateSpeakerInput input = new("tests", "lastname", new("test@tests.com"), "@tests", "tests", new("https://tests.com"));


            OperationResult<SpeakerDto> result = await service.CreateAsync(input);

            Assert.True(result.IsSuccess);
        }

        #endregion

        #region GetAllAsync

        [Fact]
        public async Task GetAllAsync_HasntSpeakers_ResultSuccess()
        {
            MockObject mockObject = new();

            SpeakerService service = new(mockObject.ApplicationDbContext);

            OperationResult<IEnumerable<SpeakerDto>> result = await service.GetAllAsync();

            Assert.True(result.IsSuccess);
        }


        [Fact]
        public async Task GetAllAsync_HasSpeakers_ResultSuccess()
        {
            MockObject mockObject = new();

            mockObject.ApplicationDbContext.Speakers.Add(
                new("tests", "tests", new("test@tests.com"), "@tests", "tests", new("https://tests.com"))
                );

            mockObject.ApplicationDbContext.SaveChanges();


            SpeakerService service = new(mockObject.ApplicationDbContext);

            OperationResult<IEnumerable<SpeakerDto>> result = await service.GetAllAsync();

            Assert.True(result.IsSuccess);
        }


        #endregion
    }
}
