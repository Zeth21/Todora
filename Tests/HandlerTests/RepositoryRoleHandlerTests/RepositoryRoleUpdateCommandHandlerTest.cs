using Application.CQRS.Commands.RepositoryRoleCommands;
using Application.CQRS.Handlers.RepositoryRoleHandlers;
using Application.CQRS.Results.RepositoryRoleResults;
using Application.Interfaces.Data.Security;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Entities;
using Domain.Enum;
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

namespace Tests.HandlerTests.RepositoryRoleHandlerTests
{
    public class RepositoryRoleUpdateCommandHandlerTest
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<IAuthorizationService> _mockAuthorizationService;
        private readonly RepositoryRoleUpdateCommandHandler _handler;

        public RepositoryRoleUpdateCommandHandlerTest()
        {
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockMapper = new Mock<IMapper>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockAuthorizationService = new Mock<IAuthorizationService>();


            var userClaims = new List<Claim>() { new Claim(ClaimTypes.NameIdentifier, "auth-user-id") };
            var identity = new ClaimsIdentity(userClaims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = claimsPrincipal };
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

            _handler = new RepositoryRoleUpdateCommandHandler(
                _mockUnitOfWork.Object,
                _mockHttpContextAccessor.Object,
                _mockAuthorizationService.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task Handle_Should_Return_Success_WhenUserIsAuthorizedAndRoleCanBeUpdated()
        {
            var command = new RepositoryRoleUpdateCommand
            {
                RepositoryRoleId = 1,
                RoleValue = RoleValues.SeniorMember
            };

            var repositoryRole = new RepositoryRole
            {
                UserId = "user-to-assign",
                RepositoryRoleId = 1,
                RoleId = (int)RoleValues.JuniorMember
            };

            var updatedDto = new RepositoryRoleUpdateCommandResult
            {
                RepositoryRoleId= repositoryRole.RepositoryRoleId,
            };

            _mockUnitOfWork.Setup(uow => uow.RepositoryRoles.GetById(command.RepositoryRoleId))
                .ReturnsAsync(repositoryRole);

            _mockAuthorizationService.Setup(auth => auth.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                repositoryRole,
                It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                    .ReturnsAsync(AuthorizationResult.Success());

            _mockUnitOfWork.Setup(uow => uow.CompleteAsync())
                .ReturnsAsync(1);

            _mockUnitOfWork.Setup(uow => uow.RepositoryRoles.FindUserRepositoryRole(repositoryRole.UserId, repositoryRole.RepositoryId))
                .ReturnsAsync(repositoryRole);

            _mockMapper.Setup(mm => mm.Map<RepositoryRoleUpdateCommandResult>(repositoryRole))
                .Returns(updatedDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSucceeded.Should().BeTrue();
            result.StatusCode.Should().Be(IntegerValues.Ok);
            result.Data.Should().NotBeNull();
            result.Data.RepositoryRoleId.Should().Be(command.RepositoryRoleId);

            _mockUnitOfWork.Verify(uow => uow.RepositoryRoles.UpdateAsync(It.IsAny<RepositoryRole>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }
    }
}
