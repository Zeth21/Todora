using Application.CQRS.Commands.UserCommands;
using Application.CQRS.Handlers.UserHandlers;
using AutoMapper;
using Domain.Entities;
using Domain.Values;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.HandlerTests.UserHandlerTests
{
    public class UserCreateCommandHandlerTest
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserCreateCommandHandler _handler;

        public UserCreateCommandHandlerTest()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            _mapperMock = new Mock<IMapper>();

            _handler = new UserCreateCommandHandler(
                _userManagerMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenUserIsCreatedAndAddedToRoleSuccessfully() 
        {
            var command = new UserCreateCommand
            {
                Email = "test@gmail.com",
                Name = "Test",
                Surname = "Test",
                UserName = "Test",
                Password = "Test"
            };

            var user = new User 
            {
                Email = "test@gmail.com",
                Name = "Test",
                Surname = "Test",
                UserName = "Test",
                PasswordHash = "Test"
            };

            _userManagerMock.Setup(um => um.FindByEmailAsync(command.Email))
                .ReturnsAsync((User)null);

            _userManagerMock.Setup(um => um.FindByNameAsync(command.UserName))
                .ReturnsAsync((User)null);

            _mapperMock.Setup(m => m.Map<User>(command))
                .Returns(user);

            _userManagerMock.Setup(um => um.CreateAsync(user, command.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(um => um.AddToRoleAsync(user, StringValues.UserRole))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSucceeded.Should().BeTrue();
            result.StatusCode.Should().Be(IntegerValues.Created);

            _userManagerMock.Verify(um => um.FindByEmailAsync(command.Email), Times.Once());

            _userManagerMock.Verify(um => um.FindByNameAsync(command.UserName), Times.Once());

            _mapperMock.Verify(m => m.Map<User>(command), Times.Once());

            _userManagerMock.Verify(um => um.CreateAsync(user, command.Password), Times.Once());

            _userManagerMock.Verify(um => um.AddToRoleAsync(user, StringValues.UserRole), Times.Once());

        }

        [Fact]
        public async Task Handle_Should_ReturnFail_WhenEmailIsAlreadyTaken() 
        {
            var command = new UserCreateCommand
            {
                Email = "test@gmail.com",
                Name = "Test",
                Surname = "Test",
                UserName = "Test",
                Password = "Test"
            };

            var user = new User
            {
                Email = "test@gmail.com",
                Name = "Test",
                Surname = "Test",
                UserName = "Test",
                PasswordHash = "Test"
            };

            _userManagerMock.Setup(um => um.FindByEmailAsync(command.Email))
                .ReturnsAsync(user);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSucceeded.Should().BeFalse();
            result.StatusCode.Should().Be(IntegerValues.BadRequest);

            _userManagerMock.Verify(um => um.FindByEmailAsync(command.Email), Times.Once());
        }

        [Fact]
        public async Task Handle_Should_ReturnFail_WhenUserNameIsAlreadyTaken() 
        {
            var command = new UserCreateCommand
            {
                Email = "test@gmail.com",
                Name = "Test",
                Surname = "Test",
                UserName = "Test",
                Password = "Test"
            };

            var user = new User
            {
                Email = "test@gmail.com",
                Name = "Test",
                Surname = "Test",
                UserName = "Test",
                PasswordHash = "Test"
            };

            _userManagerMock.Setup(um => um.FindByEmailAsync(command.Email))
                .ReturnsAsync((User)null);

            _userManagerMock.Setup(um => um.FindByNameAsync(command.UserName))
                .ReturnsAsync(user);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSucceeded.Should().BeFalse();
            result.StatusCode.Should().Be(IntegerValues.BadRequest);

            _userManagerMock.Verify(um => um.FindByEmailAsync(command.Email), Times.Once());

            _userManagerMock.Verify(um => um.FindByNameAsync(command.UserName), Times.Once());

        }

        [Fact]
        public async Task Handle_Should_ThrowException_WhenUserCreationFails()
        {
            var command = new UserCreateCommand
            {
                Email = "test@gmail.com",
                Name = "Test",
                Surname = "Test",
                UserName = "Test",
                Password = "Test"
            };

            var user = new User
            {
                Email = "test@gmail.com",
                Name = "Test",
                Surname = "Test",
                UserName = "Test",
                PasswordHash = "Test"
            };
            var identityError = new IdentityError { Description = "Passwords must be at least 6 characters." };

            _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email))
                .ReturnsAsync((User)null);

            _userManagerMock.Setup(x => x.FindByNameAsync(command.UserName))
                .ReturnsAsync((User)null);

            _mapperMock.Setup(m => m.Map<User>(command))
                .Returns(user);

            _userManagerMock.Setup(x => x.CreateAsync(user, command.Password))
                .ReturnsAsync(IdentityResult.Failed(identityError));

            await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<Exception>();

        }
    }
}
