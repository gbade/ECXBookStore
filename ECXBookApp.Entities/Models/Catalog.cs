using System.Collections.Generic;
using Newtonsoft.Json;

namespace ECXBookApp.Entities.Models
{
    public class Catalog
    {
        [JsonProperty("book")]
        public List<Book> Books { get; set; }

        public Catalog()
        {
            Books = new List<Book>();
        }
    }

    public class Root
    {
        [JsonProperty("catalog")]
        public Catalog Catalogue { get; set; }
    }
}
