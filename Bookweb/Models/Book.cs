using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookweb.Models
{
    public class Book
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public bool IsAccepted { get; set; }
    }
}
