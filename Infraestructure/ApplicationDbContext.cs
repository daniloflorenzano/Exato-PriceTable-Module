using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<Table?> Tables { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Table>(
                eb =>
                {
                    eb.HasNoKey();
                    eb.ToView("registered_tables");
                    eb.Property(v => v.Id).HasColumnName("id");
                    eb.Property(v => v.ExternalId).HasColumnName("external_id");
                    eb.Property(v => v.Name).HasColumnName("name");
                    eb.Property(v => v.Description).HasColumnName("description");
                    eb.Property(v => v.Type).HasColumnName("type");
                    eb.Property(v => v.Active).HasColumnName("active");
                    eb.Property(v => v.ExpirationDate).HasColumnName("expiration_date");
                    eb.Property(v => v.CreationDate).HasColumnName("creation_date");
                });
    }
}