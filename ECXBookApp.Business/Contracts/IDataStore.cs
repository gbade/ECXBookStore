using ECXBookApp.Entities.Models;
using System.Linq;

namespace ECXBookApp.Business.Contracts
{
    public interface IDataStore
    {
        User BorrowBook(Inventory data);
        User ReturnBook(Inventory data);
        IQueryable<Book> GetBooks();
        Archives GetUserAndBookRecord();
        User GetDefaultUser();
    }
}
