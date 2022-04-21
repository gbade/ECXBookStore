using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ECXBookApp.Entities.Models
{
    public class Book
    {
        [JsonProperty("@id")]
        public string Id { get; set; }
        [JsonProperty("author")]
        public string Author { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("genre")]
        public string Genre { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
        [JsonProperty("publish_date")]
        public DateTime PublishDate { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }

    [Keyless]
    public class BooksBorrowed
    {
        public List<Book> Book { get; set; }

        public BooksBorrowed()
        {
            Book = new List<Book>();
        }
    }

    public class Inventory
    {
        public Book Book { get; set; }
        public bool IsBorrowed { get; set; }
        public string BorrowedBy { get; set; }
        public string UserId { get; set; }
    }

    public class Archives
    {
        public List<Inventory> UserAndBookRecord { get; set; }

        public Archives()
        {
            UserAndBookRecord = new List<Inventory>();
        }
    }
}
