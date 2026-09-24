
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookSwap.Models;

public class ExchangeRequestsController : Controller
{
    private readonly BookSwapContext _context;

    public ExchangeRequestsController(BookSwapContext context)
    {
        _context = context;
    }

    // GET: EXCHANGEREQUESTS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.ExchangeRequest.ToListAsync());
    }

    // GET: EXCHANGEREQUESTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var exchangerequest = await _context.ExchangeRequest
            .FirstOrDefaultAsync(m => m.Id == id);
        if (exchangerequest == null)
        {
            return NotFound();
        }

        return View(exchangerequest);
    }

    // GET: ExchangeRequests/Create
    public IActionResult Create()
    {
        ViewData["OfferedBookId"] = new SelectList(
            _context.Book,
            "Id",
            "Title"
        );

        ViewData["RequestedBookId"] = new SelectList(
            _context.Book,
            "Id",
            "Title"
        );

        return View();
    }

    // POST: EXCHANGEREQUESTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    // POST: ExchangeRequests/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("OfferedBookId,RequestedBookId")]
    ExchangeRequest exchangeRequest)
    {
        if (ModelState.IsValid)
        {
            exchangeRequest.RequestDate = DateTime.UtcNow;
            exchangeRequest.Status = "Pending";

            _context.Add(exchangeRequest);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewData["OfferedBookId"] = new SelectList(
            _context.Book,
            "Id",
            "Title",
            exchangeRequest.OfferedBookId
        );

        ViewData["RequestedBookId"] = new SelectList(
            _context.Book,
            "Id",
            "Title",
            exchangeRequest.RequestedBookId
        );

        return View(exchangeRequest);
    }

    // GET: EXCHANGEREQUESTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var exchangerequest = await _context.ExchangeRequest.FindAsync(id);
        if (exchangerequest == null)
        {
            return NotFound();
        }
        return View(exchangerequest);
    }

    // POST: EXCHANGEREQUESTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,OfferedBookId,RequestedBookId,RequestDate,Status")] ExchangeRequest exchangerequest)
    {
        if (id != exchangerequest.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(exchangerequest);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExchangeRequestExists(exchangerequest.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(exchangerequest);
    }

    // GET: EXCHANGEREQUESTS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var exchangerequest = await _context.ExchangeRequest
            .FirstOrDefaultAsync(m => m.Id == id);
        if (exchangerequest == null)
        {
            return NotFound();
        }

        return View(exchangerequest);
    }

    // POST: EXCHANGEREQUESTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var exchangerequest = await _context.ExchangeRequest.FindAsync(id);
        if (exchangerequest != null)
        {
            _context.ExchangeRequest.Remove(exchangerequest);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ExchangeRequestExists(int? id)
    {
        return _context.ExchangeRequest.Any(e => e.Id == id);
    }
}
