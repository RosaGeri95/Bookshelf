using System;
using System.Collections.Generic;
using System.Text;

namespace Bookshelf.Models
{
    public class GrUser
    {
        public string Name { get; set; }

        public string Joined { get; set; }

        public string Image { get; set; }

        public int NumberOfFriends { get; set; }

        public int NumberOfShelves { get; set; }
    }
}
