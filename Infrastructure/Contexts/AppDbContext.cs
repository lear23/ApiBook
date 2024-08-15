

using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {

        public DbSet<BookEntity> Books { get; set; }
        public DbSet<ClientEntity> Clients { get; set; }
        public DbSet<QuoteEntity> Quotes { get; set; }  


    }
}
