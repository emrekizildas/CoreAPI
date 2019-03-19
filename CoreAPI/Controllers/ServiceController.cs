using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreAPI.DTOs;
using CoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly DatabaseContext _db;
        public ServiceController(DatabaseContext context)
        {
            _db = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookInfoDto>> GetBook(int id)
        {
            BookInfoDto book = await _db.Books
            .Select(x => new BookInfoDto() { BookID = x.BookID, Name = x.Name, AuthorName = x.Author.Name })
            .SingleOrDefaultAsync(x => x.BookID == id);

            if (book == null)
                return NotFound("İstenilen kitap bulunamadı");
            return Ok(book);
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            List<BookInfoDto> books = await _db.Books
                .Select(x => new BookInfoDto() { BookID = x.BookID, Name = x.Name, AuthorName = x.Author.Name })
                .ToListAsync();

            return Ok(books);
        }

        [HttpGet("{id}")]
        public IActionResult GetBooksByAuthor(int id)
        {
            List<BookInfoDto> books = _db.Books
                    .Where(x => x.AuthorID == id)
                    .Select(x => new BookInfoDto() { BookID = x.BookID, Name = x.Name, AuthorName = x.Author.Name })
                    .ToList();

            return Ok(books);
        }

        [HttpPost]
        public IActionResult AddAuthor(Author author)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _db.Authors.Add(author);
            _db.SaveChanges();

            return Ok("Ekleme başarılı");
        }

        [HttpGet]
        public IActionResult GetAuthors()
        {
            List<AuthorInfoDto> authors = _db.Authors
                .Select(x => new AuthorInfoDto() { AuthorID = x.AuthorID ,AuthorName = x.Name, TotalBook = x.Books.Count() })
                .ToList();
            return Ok(authors);
        }

        [HttpPost]
        public IActionResult AddBook([FromBody]Book book)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (_db.Authors.Find(book.AuthorID) == null)
                return NotFound("Yazar bulunamadı");

            _db.Books.Add(book);
            _db.SaveChanges();
            return Ok("Ekleme Başarılı");
        }

    }
}
