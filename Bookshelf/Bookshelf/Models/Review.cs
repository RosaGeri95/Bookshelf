    using System;
using System.Collections.Generic;
using System.Text;

namespace Bookshelf.Models
{
    public class Review
    {
        public ulong ID { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
