using Application.CQRS.Commands.RepositoryCommands;
using Application.CQRS.Handlers.RepositoryHandlers;
using Application.CQRS.Results.RepositoryResults;
using Application.Exceptions;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Entities;
using Domain.Values;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.HandlerTests.RepositoryHandlerTests
{
    public class RepositoryCreateCommandHandlerTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IMapper> _mapper;
        private readonly RepositoryCreateCommandHandler _handler;
        public RepositoryCreateCommandHandlerTest() 
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _mapper = new Mock<IMapper>();
            _handler = new RepositoryCreateCommandHandler(
                _unitOfWork.Object,
                _mapper.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_Success_WhenRepositoryCanBeCreated() 
        {
            var command = new RepositoryCreateCommand 
            {
                UserId = "user-to-assign",
                RepositoryTitle = "Test",
                RepositoryDescription = "Test"
            };

            var repository = new Repository
            {
                RepositoryTitle = "Test",
                RepositoryDescription = "Test",
                RepositoryUserId = "user-to-assign"
            };

            var record = new RepositoryCreateCommandResult { };

            _unitOfWork.Setup(uow => uow.Repositories.GetUserRepositoryByTitleAsync(command.UserId, command.RepositoryTitle))
                .ReturnsAsync((Repository)null);

            _mapper.Setup(m => m.Map<Repository>(command)).Returns(repository);
            _mapper.Setup(m => m.Map<RepositoryCreateCommandResult>(repository)).Returns(record);
            _unitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(2);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Data.Should().NotBeNull();
            result.IsSucceeded.Should().BeTrue();

            _unitOfWork.Verify(uow => uow.Repositories.GetUserRepositoryByTitleAsync(command.UserId,command.RepositoryTitle), Times.Once);
            _unitOfWork.Verify(uow => uow.Repositories.AddAsync(repository), Times.Once);
            _unitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Fail_WhenRepositoryCannotBeCreated() 
        {
            var command = new RepositoryCreateCommand 
            {
                UserId = "user-to-assign",
                RepositoryTitle = "Test",
                RepositoryDescription = "Test"
            };

            var repository = new Repository
            {
                RepositoryTitle = "Test",
                RepositoryDescription = "Test",
                RepositoryUserId = "user-to-assign"
            };

            _unitOfWork.Setup(uow => uow.Repositories.GetUserRepositoryByTitleAsync(command.UserId, command.RepositoryTitle))
                .ReturnsAsync(repository);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Data.Should().BeNull();
            result.IsSucceeded.Should().BeFalse();
            result.StatusCode.Should().Be(IntegerValues.BadRequest);

            _unitOfWork.Verify(uow => uow.Repositories.GetUserRepositoryByTitleAsync(command.UserId, command.RepositoryTitle), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Fail_WhenDataCouldntSaved() 
        {
            var command = new RepositoryCreateCommand
            {
                UserId = "user-to-assign",
                RepositoryTitle = "Test",
                RepositoryDescription = "Test"
            };

            var repository = new Repository
            {
                RepositoryTitle = "Test",
                RepositoryDescription = "Test",
                RepositoryUserId = "user-to-assign"
            };


            _unitOfWork.Setup(uow => uow.Repositories.GetUserRepositoryByTitleAsync(command.UserId, command.RepositoryTitle))
                .ReturnsAsync((Repository)null);

            _mapper.Setup(m => m.Map<Repository>(command)).Returns(repository);
            _unitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(0);

            await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<SaveDataException>();
        }
    }
}
