using System;
using System.Collections.Generic;
using System.Linq;
using ECXBookApp.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ECXBookApp.Entities
{
    public class ECXDbContext : DbContext
    {
        public ECXDbContext(DbContextOptions<ECXDbContext> options)
        : base(options)
        {
            LoadDefaultUser();
        }

        public DbSet<User> User { get; set; }

        public void LoadDefaultUser()
        {
            User defaultUser = new User
            {
                UserID = "147538c2-bafd-405c-9b60-41f3ed7730f0",
                UserName = "debo_a",
                Password = "passwd1234",
                FirstName = "Debo",
                LastName = "Ayan",
                BorrowedBooks = new BooksBorrowed
                {
                    Book = new List<Book>()
                    {
                        new Book
                        {
                            Id = "bk102",
                            Author = "Ralls, Kim",
                            Title = "Midnight Rain",
                            Genre = "Fantasy",
                            Price = 5.95m,
                            PublishDate = new DateTime(2000, 12, 16, 0, 0, 0),
                            Description = @"A former architect battles corporate zombies,
                               an evil sorceress,and her own childhood to become queen
                               of the world."
                        },
                        new Book
                        {
                            Id = "bk109",
                            Author = "Kress, Peter",
                            Title = "Paradox Lost",
                            Genre = "Science Fiction",
                            Price = 6.95m,
                            PublishDate = new DateTime(2000, 11, 02, 0, 0, 0),
                            Description = @"After an inadvertant trip through a Heisenberg
            		            Uncertainty Device, James Salway discovers the problems
            		            of being quantum."
                        }
                    }
                }
            };
            User.Add(defaultUser);

            defaultUser = new User
            {

                UserID = "94a54e0d-3d7a-463e-9b6a-bd9b1d0bd7c6",
                UserName = "john_doe",
                Password = "john_d1234",
                FirstName = "John",
                LastName = "Doe",
                BorrowedBooks = new BooksBorrowed
                {
                    Book = new List<Book>()
                    {
                        new Book
                        {
                            Id = "bk103",
                            Author = "Corets, Eva",
                            Title = "Maeve Ascendant",
                            Genre = "Fantasy",
                            Price = 5.95m,
                            PublishDate = new DateTime(2000, 11, 17, 0, 0, 0),
                            Description = @"After the collapse of a nanotechnology
                                society in England, the young survivors lay the
                                foundation for a new society."
                        },
                        new Book
                        {
                            Id = "bk104",
                            Author = "Corets, Eva",
                            Title = "Oberon's Legacy",
                            Genre = "Fantasy",
                            Price = 5.95m,
                            PublishDate = new DateTime(2001, 03, 10, 0, 0, 0),
                            Description = @"In post-apocalypse England, the mysterious
                                agent known only as Oberon helps to create a new life
                                for the inhabitants of London. Sequel to Maeve
                                Ascendant."
                        }
                    }
                }
            };
            User.Add(defaultUser);
        }

        public List<User> GetUsers()
        {
            return User.Local.ToList<User>();
        }
    }
}
