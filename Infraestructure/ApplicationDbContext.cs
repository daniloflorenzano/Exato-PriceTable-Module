using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<Table> Tables { get; set; }
    public DbSet<Item> Items { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}