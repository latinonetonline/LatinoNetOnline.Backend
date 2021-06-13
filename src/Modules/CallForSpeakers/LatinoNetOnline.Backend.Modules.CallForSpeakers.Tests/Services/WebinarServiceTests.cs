﻿using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Webinars;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Entities;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Services;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

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

            WebinarService service = new(mockObject.ApplicationDbContext);

            OperationResult<IEnumerable<WebinarFullDto>> result = await service.GetAllAsync();

            Assert.True(result.IsSuccess);
            Assert.False(result.Result.Any());
        }


        [Fact]
        public async Task GetAllAsync_HasWebinars_ResultSuccess()
        {
            MockObject mockObject = new();

            Proposal proposal = GetProposal();
            Uri uri = new("https://tests.com");

            mockObject.ApplicationDbContext.Proposals.Add(proposal);


            mockObject.ApplicationDbContext.Webinars.Add(
                new(proposal.Id, uri, uri, uri)
                );

            mockObject.ApplicationDbContext.SaveChanges();


            WebinarService service = new(mockObject.ApplicationDbContext);

            OperationResult<IEnumerable<WebinarFullDto>> result = await service.GetAllAsync();

            Assert.True(result.IsSuccess);
            Assert.True(result.Result.Any());
        }


        #endregion


        #region GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_WithValidId_ResultSuccess()
        {
            MockObject mockObject = new();

            Proposal proposal = GetProposal();
            Uri uri = new("https://tests.com");

            mockObject.ApplicationDbContext.Proposals.Add(proposal);

            Webinar webinar = new(proposal.Id, uri, uri, uri);
            mockObject.ApplicationDbContext.Webinars.Add(webinar);

            mockObject.ApplicationDbContext.SaveChanges();

            WebinarService service = new(mockObject.ApplicationDbContext);

            var result = await service.GetByIdAsync(webinar.Id);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ResultError()
        {
            MockObject mockObject = new();

            WebinarService service = new(mockObject.ApplicationDbContext);

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

            Proposal proposal = GetProposal();
            Uri uri = new("https://tests.com");

            mockObject.ApplicationDbContext.Proposals.Add(proposal);

            mockObject.ApplicationDbContext.SaveChanges();

            WebinarService service = new(mockObject.ApplicationDbContext);

            OperationResult<WebinarDto> result = await service.CreateAsync(new(proposal.Id, uri, uri, uri));

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task CreateAsync_WithInvalidProposalId_ResultError()
        {
            MockObject mockObject = new();

            Uri uri = new("https://tests.com");

            WebinarService service = new(mockObject.ApplicationDbContext);

            OperationResult<WebinarDto> result = await service.CreateAsync(new(Guid.NewGuid(), uri, uri, uri));

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CreateAsync_WithNullYoutubeLink_ResultError()
        {
            MockObject mockObject = new();

            Proposal proposal = GetProposal();
            Uri uri = new("https://tests.com");

            mockObject.ApplicationDbContext.Proposals.Add(proposal);

            mockObject.ApplicationDbContext.SaveChanges();

            WebinarService service = new(mockObject.ApplicationDbContext);

            OperationResult<WebinarDto> result = await service.CreateAsync(new(proposal.Id, null, uri, uri));

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CreateAsync_WithNullMeetupLink_ResultError()
        {
            MockObject mockObject = new();

            Proposal proposal = GetProposal();
            Uri uri = new("https://tests.com");

            mockObject.ApplicationDbContext.Proposals.Add(proposal);

            mockObject.ApplicationDbContext.SaveChanges();

            WebinarService service = new(mockObject.ApplicationDbContext);

            OperationResult<WebinarDto> result = await service.CreateAsync(new(proposal.Id, uri, null, uri));

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CreateAsync_WithNullFlyerLink_ResultError()
        {
            MockObject mockObject = new();

            Proposal proposal = GetProposal();
            Uri uri = new("https://tests.com");

            mockObject.ApplicationDbContext.Proposals.Add(proposal);

            mockObject.ApplicationDbContext.SaveChanges();

            WebinarService service = new(mockObject.ApplicationDbContext);

            OperationResult<WebinarDto> result = await service.CreateAsync(new(proposal.Id, uri, uri, null));

            Assert.False(result.IsSuccess);
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_WithValidId_ReturnSuccess()
        {
            MockObject mockObject = new();

            Proposal proposal = GetProposal();
            Uri uri = new("https://tests.com");

            mockObject.ApplicationDbContext.Proposals.Add(proposal);

            Webinar webinar = new(proposal.Id, uri, uri, uri);
            mockObject.ApplicationDbContext.Webinars.Add(webinar);

            mockObject.ApplicationDbContext.SaveChanges();

            WebinarService service = new(mockObject.ApplicationDbContext);

            OperationResult result = await service.DeleteAsync(webinar.Id);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task DeleteAsync_WithInalidId_ReturnError()
        {
            MockObject mockObject = new();

            WebinarService service = new(mockObject.ApplicationDbContext);

            OperationResult result = await service.DeleteAsync(Guid.NewGuid());

            Assert.False(result.IsSuccess);
        }

        #endregion-


        #region GetNextWebinarAsync

        [Fact]
        public async Task GetAllAsync_HasntWebinars_ResultError()
        {
            MockObject mockObject = new();

            WebinarService service = new(mockObject.ApplicationDbContext);

            OperationResult<WebinarFullDto> result = await service.GetNextWebinarAsync();

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task GetAllAsync_HasOneWebinar_ResultSuccess()
        {
            MockObject mockObject = new();

            Proposal proposal = GetProposal();
            Uri uri = new("https://tests.com");

            mockObject.ApplicationDbContext.Proposals.Add(proposal);

            Webinar webinar = new(proposal.Id, uri, uri, uri);
            mockObject.ApplicationDbContext.Webinars.Add(webinar);

            mockObject.ApplicationDbContext.SaveChanges();


            WebinarService service = new(mockObject.ApplicationDbContext);

            OperationResult<WebinarFullDto> result = await service.GetNextWebinarAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(webinar.Id, result.Result.Webinar.Id);
        }

        [Fact]
        public async Task GetAllAsync_HasTwoWebinar_ResultSuccess()
        {
            MockObject mockObject = new();

            Proposal proposal = GetProposal();
            Proposal proposal2 = GetProposa2();

            Uri uri = new("https://tests.com");

            mockObject.ApplicationDbContext.Proposals.Add(proposal);
            mockObject.ApplicationDbContext.Proposals.Add(proposal2);


            Webinar webinar = new(proposal.Id, uri, uri, uri);
            Webinar webinar2 = new(proposal2.Id, uri, uri, uri);

            mockObject.ApplicationDbContext.Webinars.Add(webinar);
            mockObject.ApplicationDbContext.Webinars.Add(webinar2);


            mockObject.ApplicationDbContext.SaveChanges();


            WebinarService service = new(mockObject.ApplicationDbContext);

            OperationResult<WebinarFullDto> result = await service.GetNextWebinarAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(webinar.Id, result.Result.Webinar.Id);
        }

        [Fact]
        public async Task GetAllAsync_HasThreeWebinar_ResultSuccess()
        {
            MockObject mockObject = new();

            Proposal proposal = GetProposal();
            Proposal proposal2 = GetProposa2();
            Proposal proposal3 = GetProposa3();


            Uri uri = new("https://tests.com");

            mockObject.ApplicationDbContext.Proposals.Add(proposal2);
            mockObject.ApplicationDbContext.Proposals.Add(proposal);
            mockObject.ApplicationDbContext.Proposals.Add(proposal3);

            Webinar webinar = new(proposal2.Id, uri, uri, uri);
            Webinar webinar2 = new(proposal3.Id, uri, uri, uri);
            Webinar webinar3 = new(proposal.Id, uri, uri, uri);
            mockObject.ApplicationDbContext.Webinars.Add(webinar2);
            mockObject.ApplicationDbContext.Webinars.Add(webinar3);
            mockObject.ApplicationDbContext.Webinars.Add(webinar);

            mockObject.ApplicationDbContext.SaveChanges();


            WebinarService service = new(mockObject.ApplicationDbContext);

            OperationResult<WebinarFullDto> result = await service.GetNextWebinarAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(webinar3.Id, result.Result.Webinar.Id);
        }

        #endregion

        private static Proposal GetProposal()
            => new("test", "tests", string.Empty, string.Empty, string.Empty, new(2046, 08, 18));

        private static Proposal GetProposa2()
            => new("test", "tests", string.Empty, string.Empty, string.Empty, new(2046, 09, 01));

        private static Proposal GetProposa3()
            => new("test", "tests", string.Empty, string.Empty, string.Empty, new(2046, 09, 08));
    }
}