using Microsoft.EntityFrameworkCore;
using RoomBazar.Domain.Entites;
using RoomBazar.Domain.Entites.Auths;

namespace RoomBazar.Data.DbContexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Register> Registers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Boshqa konfiguratsiyalar...
    }

}   
