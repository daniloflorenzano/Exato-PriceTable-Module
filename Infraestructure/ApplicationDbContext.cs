using Domain;
using Domain.Entities;
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
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=mysecretpassword");
    }
}