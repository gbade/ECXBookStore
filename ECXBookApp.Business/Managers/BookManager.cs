using System.Collections.Generic;
using System.Linq;
using ECXBookApp.Business.Contracts;
using ECXBookApp.Business.Utilities;
using ECXBookApp.Entities;
using ECXBookApp.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ECXBookApp.Business.Managers
{
    public class BookManager : IDataStore
    {
        private readonly string _file;
        private readonly string _xml;
        private readonly IConfigManager _config;
        private readonly Root _bookData;
        private readonly ECXDbContext _context;
        
        public BookManager(IConfigManager config, ECXDbContext context)
        {
            _context = context;

            _config = config;
            _file = _config.DataSource;

            LoadXmlDocument loader = new LoadXmlDocument();
            _xml = loader.LoadXMLObject(_file);
            _bookData = JsonConvert.DeserializeObject<Root>(_xml);
        }

        //for purpose of this exercise, in memory db
        //is used to get the first user and then cache user
        //data for use on the web app.
        public User GetDefaultUser()
        {
            var users = GetUsers();
            var defaultUser = users
                .Where(u => u.UserID == "147538c2-bafd-405c-9b60-41f3ed7730f0")
                .FirstOrDefault();

            return defaultUser;
        }

        public Archives GetUserAndBookRecord()
        {
            var archive = new Archives();
            var users = GetUsers();

            var catalog = GetBooks().ToList();
            var inventory = GetInventoryForCatalog(catalog, users);

            archive.UserAndBookRecord = inventory;

            return archive;
        }

        public User BorrowBook(Inventory data)
        {
            var users = GetUsers();
            var user = GetUser(users, data.UserId);

            if (user != null)
            {
                user.BorrowedBooks.Book.Add(data.Book);
                _context.Entry(user).State = EntityState.Modified;
            }

            return user;
        }

        public User ReturnBook(Inventory data)
        {
            var users = GetUsers();
            var user = GetUser(users, data.UserId);

            if (user != null)
            {
                var borrowedBook = user.BorrowedBooks.Book
                                    .FirstOrDefault(x => x.Id == data.Book.Id);

                if (borrowedBook != null)
                    user.BorrowedBooks.Book.Remove(borrowedBook);

                _context.Entry(user).State = EntityState.Modified;
            }

            return user;
        }

        private List<Inventory> GetInventoryForCatalog(List<Book> catalog, List<User> users)
        {
            var inventory = catalog.Select(bk => new Inventory
            {
                Book = bk,
                BorrowedBy = users.Where(u => u.BorrowedBooks.Book.Any(b => b.Id == bk.Id))
                                  .FirstOrDefault()?.UserName,
                UserId = users.Where(u => u.BorrowedBooks.Book.Any(b => b.Id == bk.Id))
                              .FirstOrDefault()?.UserID,
                IsBorrowed = users.Any(u => u.BorrowedBooks.Book.Any(b => b.Id == bk.Id))
            }).ToList();

            return inventory;
        }

        private User GetUser(List<User> users, string userId)
        {
            return users.Where(x => x.UserID == userId).FirstOrDefault();
        }

        public IQueryable<Book> GetBooks()
        {
            return _bookData.Catalogue.Books.AsQueryable();
        }

        private List<User> GetUsers()
        {
            return _context.User.Local.ToList();
        }
    }
}
