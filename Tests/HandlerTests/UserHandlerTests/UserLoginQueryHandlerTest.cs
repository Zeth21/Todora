using Application.CQRS.Handlers.UserHandlers;
using Application.CQRS.Queries.UserQueries;
using Application.Interfaces.Data.Security;
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
    public class UserLoginQueryHandlerTest
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly UserLoginQueryHandler _handler;
        private readonly Mock<IJWTGenerator> _mockJWTGenerator;

        public UserLoginQueryHandlerTest()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            _mockJWTGenerator = new Mock<IJWTGenerator>();

            _handler = new UserLoginQueryHandler(
                _mockUserManager.Object,
                _mockJWTGenerator.Object);
        }

        [Fact]
        public async Task Handler_Should_Return_Success_WhenEmailAndPasswordAreCorrect() 
        {
            var query = new UserLoginQuery 
            {
                Email = "test@gmail.com",
                Password = "password"
            };

            var user = new User 
            { 
                Name = "Test",
                Email = "test@gmail.com"
            };
            
            var roles = new List<string> { };

            var token = "Test";

            _mockUserManager.Setup(mum => mum.FindByEmailAsync(query.Email))
                .ReturnsAsync(user);
            
            _mockUserManager.Setup(mum => mum.CheckPasswordAsync(user,query.Password))
                .ReturnsAsync(true);

            _mockUserManager.Setup(mum => mum.GetRolesAsync(user))
                .ReturnsAsync(roles);

            _mockJWTGenerator.Setup(mjg => mjg.GenerateToken(user, roles))
                .Returns(token);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.IsSucceeded.Should().BeTrue();
            result.StatusCode.Should().Be(IntegerValues.Ok);
            result.Data.Should().NotBeNull();

            _mockUserManager.Verify(mum => mum.FindByEmailAsync(query.Email), Times.Once());
            _mockJWTGenerator.Verify(mjg => mjg.GenerateToken(user, roles), Times.Once());
            _mockUserManager.Verify(mjg => mjg.GetRolesAsync(user), Times.Once());
            _mockUserManager.Verify(mum => mum.CheckPasswordAsync(user, query.Password), Times.Once());
        }

        [Fact]
        public async Task Handler_Should_Return_Fail_WhenUserNotFound()
        {
            var query = new UserLoginQuery
            {
                Email = "test@gmail.com",
                Password = "password"
            };

            _mockUserManager.Setup(mum => mum.FindByEmailAsync(query.Email))
                .ReturnsAsync((User)null);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.IsSucceeded.Should().BeFalse();
            result.Data.Should().BeNull();
            result.StatusCode.Should().Be(IntegerValues.BadRequest);

            _mockUserManager.Verify(mum => mum.FindByEmailAsync(query.Email), Times.Once);
            _mockUserManager.Verify(mum => mum.CheckPasswordAsync(It.IsAny<User>(),It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public async Task Handler_Should_Return_Fail_WhenPasswordIsIncorrect() 
        {
            var query = new UserLoginQuery
            {
                Email = "test@gmail.com",
                Password = "password"
            };

            var user = new User { };

            _mockUserManager.Setup(mum => mum.FindByEmailAsync(query.Email))
                .ReturnsAsync(user);

            _mockUserManager.Setup(mum => mum.CheckPasswordAsync(user, query.Password))
                .ReturnsAsync(false);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.IsSucceeded.Should().BeFalse();
            result.Data.Should().BeNull();
            result.StatusCode.Should().Be(IntegerValues.BadRequest);

            _mockUserManager.Verify(mum => mum.FindByEmailAsync(query.Email), Times.Once());

            _mockUserManager.Verify(mum => mum.CheckPasswordAsync(user, query.Password), Times.Once());

            _mockUserManager.Verify(mum => mum.GetRolesAsync(user), Times.Never());
        }
    }
}
