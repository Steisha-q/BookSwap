using Microsoft.EntityFrameworkCore;

public class BookSwapContext(DbContextOptions<BookSwapContext> options) : DbContext(options)
{
    public DbSet<BookSwap.Models.Book> Book { get; set; } = default!;
    public DbSet<BookSwap.Models.User> User { get; set; } = default!;
    public DbSet<BookSwap.Models.ExchangeRequest> ExchangeRequest { get; set; } = default!;
}
