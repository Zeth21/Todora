using Application.CQRS.Commands.WorkTaskCommands;
using Application.CQRS.Handlers.WorkTaskHandlers;
using Application.CQRS.Results.WorkTaskResults;
using Application.Interfaces.Data.Security;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Entities;
using Domain.Values;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace Tests.HandlerTests.WorkTaskHandlerTests
{
    public class WorkTaskCreateCommandHandlerTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<IAuthorizationService> _mockAuthorizationService;
        private readonly WorkTaskCreateCommandHandler _handler;

        public WorkTaskCreateCommandHandlerTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockAuthorizationService = new Mock<IAuthorizationService>();

            _handler = new WorkTaskCreateCommandHandler(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockAuthorizationService.Object,
                _mockHttpContextAccessor.Object);
        }

        [Fact]
        public async Task Handler_Should_Return_Success_WhenUserHasAuthorizationAndTaskTitleIsValid() 
        {
            var command = new WorkTaskCreateCommand
            {
                TaskDescription = "string",
                TaskTitle = "string",
                TaskCreatedUserId = "auth-user",
                RepositoryId = 1
            };

            var repository = new Repository 
            {
                RepositoryDescription = "string",
                RepositoryTitle = "string",
                RepositoryUserId = "auth-user"
            };

            var policyResource = new WorkTask 
            {
                TaskCreatedUserId = "auth-user",
                TaskDescription = "string",
                TaskTitle = "string"
            };

            var expectedResult = new WorkTaskCreateCommandResult 
            {
                TaskCreatedUserId = "auth-user",
                TaskDescription = "string",
                TaskTitle = "string",
                TaskCreatedUserName = "string"
            };

            var authUser = new ClaimsPrincipal { };

            _mockUnitOfWork.Setup(muow => muow.Repositories.GetById(command.RepositoryId))
                .ReturnsAsync(repository);

            _mockUnitOfWork.Setup(muow => muow.WorkTasks.CheckTitleIsValid(repository.RepositoryId, command.TaskTitle))
                .ReturnsAsync(false);
            
            _mockMapper.Setup(mm => mm.Map<WorkTask>(command))
                .Returns(policyResource);

            _mockHttpContextAccessor.Setup(mhca => mhca.HttpContext.User)
                .Returns(authUser);

            _mockAuthorizationService.Setup(mas => mas.AuthorizeAsync(authUser, policyResource, It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .ReturnsAsync(AuthorizationResult.Success);

            _mockUnitOfWork.Setup(muow => muow.CompleteAsync())
                .ReturnsAsync(1);

            _mockMapper.Setup(mm => mm.Map<WorkTaskCreateCommandResult>(policyResource))
                .Returns(expectedResult);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSucceeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.StatusCode.Should().Be(IntegerValues.Created);

        }

        [Fact]
        public async Task Handle_Should_Return_Fail_WhenRepositoryNotFound()
        {
            var command = new WorkTaskCreateCommand
            {
                TaskDescription = "string",
                TaskTitle = "string",
                TaskCreatedUserId = "auth-user",
                RepositoryId = 1
            };

            _mockUnitOfWork.Setup(muow => muow.Repositories.GetById(command.RepositoryId))
                .ReturnsAsync((Repository)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSucceeded!.Should().BeFalse();
            result.Data.Should().BeNull();
            result.StatusCode.Should().Be(IntegerValues.BadRequest);

            _mockUnitOfWork.Verify(muow => muow.WorkTasks.CheckTitleIsValid(It.IsAny<int>(), It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public async Task Handle_Should_Return_Fail_WhenTaskTitleHasAlreadyTaken() 
        {
            var command = new WorkTaskCreateCommand
            {
                TaskDescription = "string",
                TaskTitle = "string",
                TaskCreatedUserId = "auth-user",
                RepositoryId = 1
            };

            var repository = new Repository 
            {
                RepositoryDescription = "string",
                RepositoryTitle = "string",
                RepositoryUserId = "auth-user",
                RepositoryId = 1
            };

            _mockUnitOfWork.Setup(muow => muow.Repositories.GetById(command.RepositoryId))
                .ReturnsAsync(repository);

            _mockUnitOfWork.Setup(muow => muow.WorkTasks.CheckTitleIsValid(repository.RepositoryId, command.TaskTitle))
                .ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSucceeded!.Should().BeFalse();
            result.Data.Should().BeNull();
            result.StatusCode.Should().Be(IntegerValues.BadRequest);

            _mockAuthorizationService.Verify(mas => mas.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                It.IsAny<IEnumerable<IAuthorizationRequirement>>()), Times.Never());
        }

        [Fact]
        public async Task Handle_Should_Return_Unauthorized_WhenUserIsNotAuthorized() 
        {

            var command = new WorkTaskCreateCommand
            {
                TaskDescription = "string",
                TaskTitle = "string",
                TaskCreatedUserId = "auth-user",
                RepositoryId = 1
            };

            var repository = new Repository
            {
                RepositoryDescription = "string",
                RepositoryTitle = "string",
                RepositoryUserId = "auth-user"
            };

            var policyResource = new WorkTask
            {
                TaskCreatedUserId = "auth-user",
                TaskDescription = "string",
                TaskTitle = "string"
            };

            var authUser = new ClaimsPrincipal { };

            _mockUnitOfWork.Setup(muow => muow.Repositories.GetById(command.RepositoryId))
                .ReturnsAsync(repository);

            _mockUnitOfWork.Setup(muow => muow.WorkTasks.CheckTitleIsValid(repository.RepositoryId, command.TaskTitle))
                .ReturnsAsync(false);

            _mockMapper.Setup(mm => mm.Map<WorkTask>(command))
                .Returns(policyResource);

            _mockHttpContextAccessor.Setup(mhca => mhca.HttpContext.User)
                .Returns(authUser);

            _mockAuthorizationService.Setup(mas => mas.AuthorizeAsync(authUser, policyResource, It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .ReturnsAsync(AuthorizationResult.Failed);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSucceeded.Should().BeFalse();
            result.Data.Should().BeNull();
            result.StatusCode.Should().Be(IntegerValues.Unauthorized);

            _mockUnitOfWork.Verify(muow => muow.WorkTasks.AddAsync(It.IsAny<WorkTask>()), Times.Never());
                
        }
    }
}
