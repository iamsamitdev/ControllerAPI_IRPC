using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControllerAPI.Controllers;

// สร้างตัวอย่าง model สำหรับการส่งค่า
public class BookModel
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
}

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class BookController : ControllerBase
{

    // กำหนดค่าเริ่มต้นให้กับ model
    private static List<BookModel> books = new List<BookModel>
    {
        new BookModel { Id = 1, Title = "Book1", Author = "Author1" },
        new BookModel { Id = 2, Title = "Book2", Author = "Author2" },
        new BookModel { Id = 3, Title = "Book3", Author = "Author3" },
    };

    // Read All books
    [HttpGet("books")]
    public IActionResult GetBooks()
    {
        return Ok(books);
    }

    // Read book by id
    [HttpGet("books/{id}")]
    public IActionResult GetBook(int id)
    {
        var book = books.FirstOrDefault(x => x.Id == id); // LINQ stand for Language Integrated Query
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }

    // Create book
    [HttpPost("books")]
    public IActionResult CreateBook([FromBody] BookModel book)
    {
        books.Add(book);
        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
    }

    // Update book
    [HttpPut("books/{id}")]
    public IActionResult UpdateBook(int id, [FromBody] BookModel book)
    {
        // หา index ของ book ที่ต้องการแก้ไข
        var index = books.FindIndex(x => x.Id == id);

        // ถ้าไม่พบข้อมูล
        if (index < 0)
        {
            // ส่งข้อมูลกลับไป 404 Not Found
            return NotFound();
        }
        // แทนที่ข้อมูลใน index ด้วยข้อมูลใหม่
        books[index] = book;

        // ส่งข้อมูลกลับไป
        return Ok(book);
    }

    // Delete book
    [HttpDelete("books/{id}")]
    public IActionResult DeleteBook(int id)
    {
        // หา index ของ book ที่ต้องการลบ
        var index = books.FindIndex(x => x.Id == id);

        // ถ้าไม่พบข้อมูล
        if (index < 0)
        {
            // ส่งข้อมูลกลับไป 404 Not Found
            return NotFound();
        }
        // ลบข้อมูลที่ index ที่พบ
        books.RemoveAt(index);

        // ส่งข้อมูลกลับไป
        return Ok();
    }

    [HttpGet]
    public IActionResult Index()
    {
        return Ok( new { message = "Book Index" });
    }

    // การส่งค่าแบบ path parameter
    // http://localhost:5285/api/book/book1/100
    [HttpGet("{name}/{price}")]
    public IActionResult GetBook(string name, string price)
    {
        return Ok( new { message = $"Get Book {name} Price {price}" });
    }

    // การส่งค่าแบบ query string
    // http://localhost:5285/api/book/search?name=book1&price=100
    [HttpGet("search")]
    public IActionResult SearchBook(
        [FromQuery] string name, [FromQuery] string price
    )
    {
        return Ok( new { message = $"Search Book {name} Price {price}" });
    }
}