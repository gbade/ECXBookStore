using System;
using System.Collections.Generic;
using ECXBookApp.Entities.Models;

namespace ECXBookApp.Models
{
    public class BooksViewModel
    {
        public Inventory BooksData { get; set; }

        public BooksViewModel()
        {
            BooksData = new Inventory();
        }
    }
}
