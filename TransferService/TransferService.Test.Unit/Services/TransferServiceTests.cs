using System;
using FluentValidation;
using Moq;
using TransferService.Domain;
using TransferService.Domain.Exceptions;
using TransferService.Repository;
using TransferService.Repository.UoW;
using TransferService.Test.Unit.Mocks;
using Xunit;

namespace TransferService.Test.Unit.Services
{
    public class TransferServiceTests
    {
        readonly Mock<ITransferRepository> transferRepositoryMock;
        readonly Mock<IUnitOfWork> unitOfWorkMock;
        readonly Mock<IValidator<Transfer>> validatorkMock;
        readonly Mock<IUserRepository> userRepositoryMock;
        readonly Mock<IEntryRepository> entryRepositoryMock;
        readonly Mock<IBankAccountRepository> bankAccountRepositoryMock;

        readonly Service.TransferService service;

        readonly Guid userId1 = new Guid("5489A967-9320-4350-E6FC-08D5CC8498F3");
        readonly Guid userId2 = new Guid("8d94051f-ac29-4b04-b8af-acfe2009af6d");
        readonly Guid userId3 = new Guid("f9f7263c-2a6e-43f5-a239-b0d1eb426f1f");

        public TransferServiceTests()
        {
            transferRepositoryMock = new Mock<ITransferRepository>();
            unitOfWorkMock = new Mock<IUnitOfWork>();
            validatorkMock = new Mock<IValidator<Transfer>>();
            userRepositoryMock = new Mock<IUserRepository>();
            entryRepositoryMock = new Mock<IEntryRepository>();
            bankAccountRepositoryMock = new Mock<IBankAccountRepository>();

            service = new Service.TransferService(transferRepositoryMock.Object,
                unitOfWorkMock.Object,
                validatorkMock.Object,
                userRepositoryMock.Object,
                entryRepositoryMock.Object,
                bankAccountRepositoryMock.Object);

            userRepositoryMock.Setup(repo => repo.GetUser(11111)).Returns(() =>
            {
                return UserMock.GetUser1();
            });

            userRepositoryMock.Setup(repo => repo.GetUser(22222)).Returns(() =>
            {
                return UserMock.GetUser2();
            });

            transferRepositoryMock.Setup(repo => repo.Insert(It.IsAny<Transfer>())).Returns(() =>
            {
                return new Transfer()
                {
                    Id = new Guid("4c046d1f-a991-4c6f-8be2-47b4c466f028"),
                    UserOrigin = UserMock.GetUser1(),
                    UserDestination = UserMock.GetUser2(),
                    Value = 100
                };
            });

        }

        [Fact]
        public void ShoudNotTransferWhenOriginUserIsNotFound()
        {
            //arrange

            //act
            Action act = () => service.Transfer(userId1, 00000, 22222, 100);

            //assert
            var exception = Assert.Throws<TransferServiceException>(act);
            Assert.Equal("Conta de origem não encontrada.", exception.Message);
        }

        [Fact]
        public void ShoudNotTransferWhenDestinationUserIsNotFound()
        {
            //arrange

            //act
            Action act = () => service.Transfer(userId1, 11111, 00000, 100);

            //assert
            var exception = Assert.Throws<TransferServiceException>(act);
            Assert.Equal("Conta de destino não encontrada.", exception.Message);
        }
        
        [Fact]
        public void ShoudNotTransferWhenRequesterIsNotOwner()
        {
            //arrange

            //act
            Action act = () => service.Transfer(userId3, 11111, 22222, 100);

            //assert
            var exception = Assert.Throws<TransferServiceException>(act);
            Assert.Equal("Essa conta de origem não é sua. Operação cancelada.", exception.Message);
        }

        [Fact]
        public void ShoudNotTransferWhenValueIsLessThanZero()
        {
            //arrange

            //act
            Action act = () => service.Transfer(userId1, 11111, 22222, -1);

            //assert
            var exception = Assert.Throws<TransferServiceException>(act);
            Assert.Equal("O Valor deve estar entre 0 e 10 mil.", exception.Message);
        }

        [Fact]
        public void ShoudNotTransferWhenValueIsGreaterThan10k()
        {
            //arrange

            //act
            Action act = () => service.Transfer(userId1, 11111, 22222, 10001);

            //assert
            var exception = Assert.Throws<TransferServiceException>(act);
            Assert.Equal("O Valor deve estar entre 0 e 10 mil.", exception.Message);
        }

        [Fact]
        public void ShoudNotTransferWhenBalanceIsNotEnogh()
        {
            //arrange

            //act
            Action act = () => service.Transfer(userId1, 11111, 22222, 9000);

            //assert
            var exception = Assert.Throws<TransferServiceException>(act);
            Assert.Equal("Você não tem saldo suficiente para essa operação.", exception.Message);
        }

        [Fact]
        public void ShoudTransferWhenAllOk ()
        {
            //arrange

            //act
            var result = service.Transfer(userId1, 11111, 22222, 100);

            //assert
            Assert.Equal(new Guid("4c046d1f-a991-4c6f-8be2-47b4c466f028"), result);
        }


    }
}
