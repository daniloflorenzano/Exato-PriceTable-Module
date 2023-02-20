using System.Text.Json;
using Domain.Entities;
using Domain.Primitives;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<Table?> Tables { get; set; }
    public DbSet<Item> Items { get; set; }

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
            
            modelBuilder
                .Entity<Item>(eb =>
                {
                    eb.HasNoKey();
                    eb.Property(i => i.Id).HasColumnName("id");
                    eb.Property(i => i.ExternalId).HasColumnName("external_id");
                    eb.Property(i => i.Description).HasColumnName("description");
                    eb.Property(i => i.Price).HasColumnName("price").HasConversion(
                        p => JsonSerializer.Serialize(p, new JsonSerializerOptions()),
                        s => JsonSerializer.Deserialize<Price>(s, new JsonSerializerOptions())!
                        );
                    eb.Property(i => i.PurchaseDate).HasColumnName("purchase_date");
                });
    }
}