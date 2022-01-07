using Bookweb.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookweb.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            DatabaseContext database = new DatabaseContext();
            return View(database.Books);
        }

        public IActionResult Add()
        {
            DatabaseContext database = new DatabaseContext();
            return View(database.Books);
        }

        public IActionResult CommitAdd(string productTitle, string productAuthor, string productDescription)
        {
            DatabaseContext database = new DatabaseContext();

            database.Books.Add(new Book
            {
                ID = database.Books.Count()+1,
                Title = productTitle,
                Author = productAuthor,
                Description = productDescription
            });
            database.SaveChanges();
            return RedirectToAction("List");
        }

        public IActionResult Remove(int id)
        {
            DatabaseContext database = new DatabaseContext();
            database.Books.Remove(database.Books.First(p => p.ID == id));
            database.SaveChanges();
            return RedirectToAction("List");
        }

        public IActionResult SearchByTitle(string title)
        {
            DatabaseContext database = new DatabaseContext();
            return View(database.Books.First(b => b.Title == title));
        }
    }
}
