﻿using Moq;
using TransferService.Domain;
using TransferService.Domain.Common;
using TransferService.Domain.Enums;
using TransferService.Domain.Validators;
using TransferService.Repository;
using TransferService.Repository.UoW;
using TransferService.Service;
using TransferService.Service.Muambator;
using TransferService.Service.Upload;
using TransferService.Test.Unit.Mocks;
using System;
using System.Text;
using System.Threading;
using Xunit;

namespace TransferService.Test.Unit.Services
{
    public class BookServiceTests
    {
        readonly Mock<IBookService> bookServiceMock;
        readonly Mock<IUploadService> uploadServiceMock;
        readonly Mock<IBookRepository> bookRepositoryMock;
        readonly Mock<IBooksEmailService> bookEmailService;
        readonly Mock<IUnitOfWork> unitOfWorkMock;
        readonly Mock<IBookUserService> bookUserServiceMock;

        public BookServiceTests()
        {
            // Definindo quais serão as classes mockadas
            bookServiceMock = new Mock<IBookService>();
            uploadServiceMock = new Mock<IUploadService>();
            unitOfWorkMock = new Mock<IUnitOfWork>();
            bookRepositoryMock = new Mock<IBookRepository>();
            bookEmailService = new Mock<IBooksEmailService>();
            bookUserServiceMock = new Mock<IBookUserService>();

            bookRepositoryMock.Setup(repo => repo.Insert(It.IsAny<Book>())).Returns(() =>
            {
                return BookMock.GetLordTheRings();
            });

            uploadServiceMock.Setup(service => service.UploadImage(null, null, null));

            bookServiceMock.Setup(service => service.Insert(It.IsAny<Book>())).Verifiable();
        }

        [Fact]
        public void AddBook()
        {
            Thread.CurrentPrincipal = new UserMock().GetClaimsUser();
            var service = new BookService(bookRepositoryMock.Object, 
                unitOfWorkMock.Object, new BookValidator(),
                uploadServiceMock.Object, bookEmailService.Object);
            Result<Book> result = service.Insert(new Book()
            {
                Title = "Lord of the Rings",
                Author = "J. R. R. Tolkien",
                ImageName = "lotr.png",
                ImageBytes = Encoding.UTF8.GetBytes("STRINGBASE64"),
                FreightOption = FreightOption.City,
                CategoryId = Guid.NewGuid()
                
            });
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

    }
}
