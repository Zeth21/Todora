using Application.CQRS.Commands.RepositoryCommands;
using Application.CQRS.Handlers.RepositoryHandlers;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Entities;
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

            _unitOfWork.Setup(uow => uow.Repositories.GetUserRepositoryByTitleAsync(command.UserId, command.RepositoryTitle))
                .ReturnsAsync((Repository)null);


        }
    }
}
