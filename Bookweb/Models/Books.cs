using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookweb.Models
{
    public class Books
    {
        public static List<Book> Value { get; set; }

        static Books()
        {
            Value = new List<Book>();
        }
    }
}
