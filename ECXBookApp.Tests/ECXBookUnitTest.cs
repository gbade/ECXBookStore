using Xunit;
using ECXBookApp.Business.Contracts;
using Moq;
using Microsoft.EntityFrameworkCore;
using ECXBookApp.Entities;
using ECXBookApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECXBookApp.Tests
{
    public class ECXBookUnitTest
    {
        private readonly Mock<IDataStore> _storeMock = new Mock<IDataStore>();
        private readonly Mock<Book> xmlMock = new Mock<Book>();
        private readonly Mock<Archives> catalogMock = new Mock<Archives>();
        private readonly Mock<User> userMock = new Mock<User>();
        private readonly DbContextOptions<ECXDbContext> options;

        public ECXBookUnitTest()
        {
            var dbName = "ECXBookStore" + DateTime.Now.ToFileTimeUtc();
            options = new DbContextOptionsBuilder<ECXDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)       
                .Options;
        }

        [Fact]
        public void WhenInMemoryDBIsInitializedEntriesShouldBeInserted()
        {
            //Arrange
            using(var context = new ECXDbContext(options))
            {
                var data = context.User.Local;
                var size = context.User.Local.Count;

                //Assert
                Assert.NotNull(data);
                Assert.Equal(2, size);
            }
        }

        [Fact]
        public void TestInMemoryDbInsertNewUser_ShouldIncreaseCountByOne()
        {
            using (var context = new ECXDbContext(options))
            {
                //Arrange
                User user = new User
                {
                    UserID = "59aae14f-2a66-4a9b-8b14-a098569f76e1",
                    UserName = "jane_doe",
                    Password = "jane_d234",
                    FirstName = "Jane",
                    LastName = "Doe",
                    BorrowedBooks = new BooksBorrowed
                    {
                        Book = new List<Book>()
                        {
                            new Book
                            {
                                Id = "bk112",
                                Author = "Galos, Mike",
                                Title = "Visual Studio 7: A Comprehensive Guide",
                                Genre = "Computer",
                                Price = 49.95m,
                                PublishDate = new DateTime(2000, 04, 16, 0, 0, 0),
                                Description = @"Microsoft Visual Studio 7 is explored in depth,
                                    looking at how Visual Basic, Visual C++, C#, and ASP+ are
                                    integrated into a comprehensive development
                                    environment."
                            }
                        }
                    }
                };

                //Act
                context.User.Add(user);

                //Assert
                Assert.Equal(3, context.User.Local.Count);
            }
        }

        [Fact]
        public void WhenUserBorrowsABookThenItShouldUpdateInventory()
        {
            //Arrange
            var inventory = new Inventory
            {
                UserId = "147538c2-bafd-405c-9b60-41f3ed7730f0",
                Book = new Book
                {
                    Id = "bk107",
                    Author = "Thurman, Paula",
                    Title = "Splish Splash",
                    Genre = "Romance",
                    Price = 4.95m,
                    PublishDate = new DateTime(2000, 11, 02, 0, 0, 0),
                    Description = @"A deep sea diver finds true love twenty
                            thousand leagues beneath the sea."
                }
            };
            _storeMock.Setup(x => x.BorrowBook(inventory)).Returns(userMock.Object);
            var service = _storeMock.Object;

            //Act
            var borrowedBook = service.BorrowBook(inventory);

            //Assert
            Assert.NotNull(borrowedBook);
            Assert.Equal(userMock.Object, borrowedBook);
        }

        [Fact]
        public void WhenUserReturnsABookThenInventoryShouldBeUpdated()
        {
            //Arrange
            var inventory = new Inventory
            {
                UserId = "147538c2-bafd-405c-9b60-41f3ed7730f0",
                Book = new Book
                {
                    Id = "bk107",
                    Author = "Thurman, Paula",
                    Title = "Splish Splash",
                    Genre = "Romance",
                    Price = 4.95m,
                    PublishDate = new DateTime(2000, 11, 02, 0, 0, 0),
                    Description = @"A deep sea diver finds true love twenty
                            thousand leagues beneath the sea."
                }
            };
            _storeMock.Setup(x => x.ReturnBook(inventory)).Returns(userMock.Object);
            var service = _storeMock.Object;

            //Act
            var returnedBook = service.ReturnBook(inventory);

            //Assert
            Assert.NotNull(returnedBook);
            Assert.Equal(userMock.Object, returnedBook);
        }

        [Fact]
        public void WhenXmlDataSourceIsInitializedXmlDataShouldBeRead()
        {
            //Arrange
            List<Book> books = new List<Book>();
            books.Add(xmlMock.Object);
            var queryableBooks = books.AsQueryable();
            _storeMock.Setup(x => x.GetBooks()).Returns(queryableBooks);

            var service = _storeMock.Object;

            //Act
            var xmlData = service.GetBooks();

            //Assert
            Assert.NotNull(xmlData);
            Assert.Equal(queryableBooks, xmlData);

        }

        [Fact]
        public void GetUserAndBookRecord_ShouldGetWhoHasBorrowedWhichBooksFromCatalog()
        {
            //Arrange
            _storeMock.Setup(x => x.GetUserAndBookRecord()).Returns(catalogMock.Object);
            var service = _storeMock.Object;

            //Act
            var record = service.GetUserAndBookRecord();

            //Assert
            Assert.NotNull(record);
            Assert.Equal(catalogMock.Object, record);

        }
    }
}
