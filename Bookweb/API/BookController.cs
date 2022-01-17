using Bookweb.Helpers;
using Bookweb.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bookweb.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        DatabaseContext database = new DatabaseContext();
        
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<Book> Get()
        {
            return database.Books;
        }

        [HttpGet("/SearchByTitle/{title}")]
        public IEnumerable<Book> GetBooksByTitle(string title)
        {
            string booktitle = title;
            if (!string.IsNullOrEmpty(booktitle))
            {
                return database.Books.Where(b => b.Title.ToLower().Contains(booktitle.ToLower()))
                .Union(database.Books.Where(b => b.Author.ToLower().Contains(booktitle.ToLower())));

            }
            else
            {
                return Get();
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public Book Get(int id)
        {
            return database.Books.First(b => b.ID == id);
        }

        // POST api/<ValuesController>
        [HttpPost]
        [ApiKeyAuth]
        public void Post([FromBody] Book value)
        {
            if (value != null)
            {
                database.Books.Add(value);
                database.SaveChanges();
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        [ApiKeyAuth]
        public void Delete(int id)
        {
            if (database.Books.Contains(database.Books.First(b => b.ID == id)))
            {
                database.Books.Remove(database.Books.First(b => b.ID == id));
                database.SaveChanges();
            }
        }
    }
}
