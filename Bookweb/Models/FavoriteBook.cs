using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookweb.Models
{
    public class FavoriteBook:Book
    {
        public string HolderId { get; set; }
    }
}
