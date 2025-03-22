using Microsoft.EntityFrameworkCore;
using RistoranteAPI.Models;

namespace RistoranteAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    public DbSet<Reservation> Reservations { get; set; }
}
