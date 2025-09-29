using Application.CQRS.Commands.RepositoryRoleCommands;
using Application.CQRS.Handlers.RepositoryRoleHandlers;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Entities;
using Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using FluentAssertions;
using System.Security.Claims;
using Xunit;
using Domain.Values;

namespace Application.Tests.RepositoryRoleHandlerTests
{
    public class RepositoryRoleCreateCommandHandlerTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IAuthorizationService> _mockAuthService;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly RepositoryRoleCreateCommandHandler _handler;

        public RepositoryRoleCreateCommandHandlerTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();

            var userStoreMock = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            _mockAuthService = new Mock<IAuthorizationService>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var userClaims = new List<Claim>() { new Claim(ClaimTypes.NameIdentifier, "auth-user-id") };
            var identity = new ClaimsIdentity(userClaims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = claimsPrincipal };
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

            _handler = new RepositoryRoleCreateCommandHandler(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockUserManager.Object,
                _mockAuthService.Object,
                _mockHttpContextAccessor.Object
            );
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenUserIsAuthorizedAndRoleCanBeCreated()
        {
            var command = new RepositoryRoleCreateCommand { UserId = "user-to-assign", RepositoryId = 1, RoleName = RoleValues.Manager };
            var userToAssign = new User { Id = "user-to-assign", UserName = "testuser" };
            var repository = new Repository { RepositoryId = 1, RepositoryDescription = "MockTest", RepositoryTitle = "MockTest", RepositoryUserId = "user-to-assign" };

            _mockUserManager.Setup(um => um.FindByIdAsync(command.UserId)).ReturnsAsync(userToAssign);
            _mockUnitOfWork.Setup(uow => uow.Repositories.GetById(command.RepositoryId)).ReturnsAsync(repository);

            _mockAuthService.Setup(auth => auth.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                repository,
                It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                        .ReturnsAsync(AuthorizationResult.Success());

            _mockUnitOfWork.Setup(uow => uow.RepositoryRoles.FindUserRepositoryRole(command.UserId, command.RepositoryId))
                           .ReturnsAsync((RepositoryRole)null);

            _mockUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(1);


            var result = await _handler.Handle(command, CancellationToken.None);


            result.IsSucceeded.Should().BeTrue();
            result.StatusCode.Should().Be(201);
            _mockUnitOfWork.Verify(uow => uow.RepositoryRoles.AddAsync(It.IsAny<RepositoryRole>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ReturnForbidden_WhenUserIsNotAuthorized()
        {
            var command = new RepositoryRoleCreateCommand { UserId = "user-to-assign", RepositoryId = 1, RoleName = RoleValues.Manager };
            var userToAssign = new User { Id = "user-to-assign" };
            var repository = new Repository { RepositoryId = 1, RepositoryDescription = "MockTest", RepositoryTitle = "MockTest", RepositoryUserId = "user-to-assign" };

            _mockUserManager.Setup(um => um.FindByIdAsync(command.UserId)).ReturnsAsync(userToAssign);
            _mockUnitOfWork.Setup(uow => uow.Repositories.GetById(command.RepositoryId)).ReturnsAsync(repository);

            _mockAuthService.Setup(auth => auth.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                repository,
                It.IsAny<IEnumerable<IAuthorizationRequirement>>())) 
                        .ReturnsAsync(AuthorizationResult.Failed());

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSucceeded.Should().BeFalse();
            result.StatusCode.Should().Be(403);

            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_ReturnFail_WhenRoleAlreadyExists()
        {
            var command = new RepositoryRoleCreateCommand { UserId = "user-to-assign", RepositoryId = 1, RoleName = RoleValues.Manager };
            var userToAssign = new User { Id = "user-to-assign" };
            var repository = new Repository { RepositoryId = 1, RepositoryDescription = "MockTest", RepositoryTitle = "MockTest", RepositoryUserId = "user-to-assign" };
            var existingRole = new RepositoryRole { UserId = "user-to-assign" };

            _mockUserManager.Setup(um => um.FindByIdAsync(command.UserId)).ReturnsAsync(userToAssign);
            _mockUnitOfWork.Setup(uow => uow.Repositories.GetById(command.RepositoryId)).ReturnsAsync(repository);
            _mockAuthService.Setup(auth => auth.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                repository,
                It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                        .ReturnsAsync(AuthorizationResult.Success());

            _mockUnitOfWork.Setup(uow => uow.RepositoryRoles.FindUserRepositoryRole(command.UserId, command.RepositoryId))
                           .ReturnsAsync(existingRole);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSucceeded.Should().BeFalse();
            result.Message.Should().Be(StringValues.CreateFailHasRecord);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Never);
        }
    }
}