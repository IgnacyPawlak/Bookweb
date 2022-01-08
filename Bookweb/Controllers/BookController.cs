using Bookweb.Areas.Identity.Data;
using Bookweb.Data;
using Bookweb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookweb.Controllers
{
    public class BookController : Controller
    {
        private DbContextOptions<BookwebContext> context;

        private UserManager<BookwebUser> userManager;

        public BookController(DbContextOptions<BookwebContext> context, UserManager<BookwebUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

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

        public IActionResult FavoriteList()
        {
            BookwebContext ctx = new BookwebContext(context);
            return View(ctx.FavoriteBooks);
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

        public IActionResult AddToFavorite(int id)
        {
            BookwebContext ctx = new BookwebContext(context);
            DatabaseContext database = new DatabaseContext();
            if (!String.IsNullOrEmpty(userManager.GetUserId(User)))
            {
                ctx.FavoriteBooks.Add(new FavoriteBook
                {
                    ID = database.Books.First(b => b.ID == id).ID,
                    Title = database.Books.First(b => b.ID == id).Title,
                    Author = database.Books.First(b => b.ID == id).Author,
                    Description = database.Books.First(b => b.ID == id).Description,
                    HolderId = userManager.GetUserId(User)
                });
                ctx.SaveChanges();
            }
            return RedirectToAction("FavoriteList");
        }

        [Authorize(Roles ="ADMIN")]
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
