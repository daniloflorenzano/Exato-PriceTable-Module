using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}