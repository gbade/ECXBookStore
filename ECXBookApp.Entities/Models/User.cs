using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ECXBookApp.Entities.Models
{
    public class User
    {
        [JsonProperty("id")]
        public string UserID { get; set; }
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [NotMapped]
        [JsonProperty("books_borrowed")]
        public BooksBorrowed BorrowedBooks { get; set; }
    }

    public class Users
    {
        public List<User> User { get; set; }
    }

    public class UserRoot
    {
        public Users Users { get; set; }
    }
}
