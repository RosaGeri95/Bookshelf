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
    }
}
