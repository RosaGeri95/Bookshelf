using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Bookshelf.Models
{
    public class Book
    {
        public string Author { get; set; }

        public string BookTitle { get; set; }

        public int PublicationYear { get; set; }

        public double Rating { get; set; }

        public string SmallImageURL { get; set; }

        public string ImageURL { get; set; }
    }
}
