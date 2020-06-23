using TransferService.Domain;
using TransferService.Domain.Enums;
using TransferService.Helper;
using System;
using System.Collections.Generic;
using Xunit;

namespace TransferService.Test.Unit.Domain
{
    public class BookTests
    {
        [Fact]
        public void BookStatusWaitingApproval()
        {
            var book = new Book();
            Assert.Equal(BookStatus.WaitingApproval, book.Status);
        }

        [Fact]
        public void TotalInteresedShouldBeZeroIfBookUsersIsNull()
        {
            var book = new Book();
            Assert.Equal(0, book.TotalInterested());
        }

        [Fact]
        public void TotalInteresedShouldBeEqualsTheBookUsersLength()
        {
            var bookUsers = new List<BookUser>();
            bookUsers.Add(new BookUser());
            bookUsers.Add(new BookUser());

            var book = new Book
            {
                BookUsers = bookUsers
            };

            Assert.Equal(2, book.TotalInterested());
        }

        [Fact]
        public void DaysInShowCaseShouldBeTheDifferenceBetweenTodayAndTheCreationDate()
        {
            int expectedDays = 5;
            var book = new Book
            {
                CreationDate = DateTime.Now.AddDays(expectedDays * -1)
            };

            Assert.Equal(expectedDays, book.DaysInShowcase());
        }

        [Fact]
        public void MayChooseWinnerWhenChooseDateIsYesterday()
        {
            var book = new Book { ChooseDate = DateTimeHelper.GetTodaySaoPaulo().AddDays(-1) };
            Assert.True(book.MayChooseWinner());
        }

        [Fact]
        public void MayChooseWinnerWhenChooseDateIsToday()
        {
            var book = new Book { ChooseDate = DateTimeHelper.GetTodaySaoPaulo() };
            Assert.True(book.MayChooseWinner());
        }

        [Fact]
        public void MayNotChooseWinnerWhenChooseDateIsTomorrow()
        {
            var book = new Book { ChooseDate = DateTimeHelper.GetTodaySaoPaulo().AddDays(1) };
            Assert.False(book.MayChooseWinner());
        }
    }
}