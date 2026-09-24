using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookSwap.Models;

public class BooksController : Controller
{
    private readonly BookSwapContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public BooksController(
        BookSwapContext context,
        IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    // GET: Books
    public async Task<IActionResult> Index()
    {
        var books = _context.Book
            .Include(b => b.User);

        return View(await books.ToListAsync());
    }

    // GET: Books/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Book
    .Include(b => b.User)
    .FirstOrDefaultAsync(m => m.Id == id);

        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // GET: Books/Create
    public IActionResult Create()
    {
        ViewData["UserId"] = new SelectList(
        _context.User,
        "Id",
        "Email"
    );

        return View();
    }

    // POST: Books/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Title,Author,Genre,PublicationYear,Publisher,ISBN,Language,Description,Condition,City,UserId")]
        Book book,
        IFormFile? imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(
                    _webHostEnvironment.WebRootPath,
                    "images",
                    "books");

                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName =
                    Guid.NewGuid().ToString() +
                    Path.GetExtension(imageFile.FileName);

                string filePath = Path.Combine(
                    uploadsFolder,
                    uniqueFileName);

                using (var fileStream = new FileStream(
                    filePath,
                    FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                book.ImagePath = "/images/books/" + uniqueFileName;
            }

            _context.Add(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        ViewData["UserId"] = new SelectList(
    _context.User,
    "Id",
    "Email",
    book.UserId
);

        return View(book);
    }

    // GET: Books/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Book
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // POST: Books/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("Id,Title,Author,Genre,PublicationYear,Publisher,ISBN,Language,Description,Condition,City")]
    Book book,
        IFormFile? imageFile)
    {
        if (id != book.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var existingBook = await _context.Book.FindAsync(id);

                if (existingBook == null)
                {
                    return NotFound();
                }

                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.Genre = book.Genre;
                existingBook.PublicationYear = book.PublicationYear;
                existingBook.Publisher = book.Publisher;
                existingBook.ISBN = book.ISBN;
                existingBook.Language = book.Language;
                existingBook.Description = book.Description;
                existingBook.Condition = book.Condition;
                existingBook.City = book.City;

                if (imageFile != null && imageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(
                        _webHostEnvironment.WebRootPath,
                        "images",
                        "books");

                    Directory.CreateDirectory(uploadsFolder);

                    if (!string.IsNullOrEmpty(existingBook.ImagePath))
                    {
                        string oldImagePath = Path.Combine(
                            _webHostEnvironment.WebRootPath,
                            existingBook.ImagePath.TrimStart('/'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    string uniqueFileName =
                        Guid.NewGuid().ToString() +
                        Path.GetExtension(imageFile.FileName);

                    string filePath = Path.Combine(
                        uploadsFolder,
                        uniqueFileName);

                    using (var fileStream = new FileStream(
                        filePath,
                        FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    existingBook.ImagePath =
                        "/images/books/" + uniqueFileName;
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        book.User = await _context.User
            .FirstOrDefaultAsync(u => u.Id == book.UserId);

        return View(book);
    }


    // GET: Books/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Book
            .Include(b => b.User)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }


    // POST: Books/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var book = await _context.Book.FindAsync(id);

        if (book != null)
        {
            if (!string.IsNullOrEmpty(book.ImagePath))
            {
                string imagePath = Path.Combine(
                    _webHostEnvironment.WebRootPath,
                    book.ImagePath.TrimStart('/'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Book.Remove(book);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    private bool BookExists(int? id)
    {
        return _context.Book.Any(e => e.Id == id);
    }
}