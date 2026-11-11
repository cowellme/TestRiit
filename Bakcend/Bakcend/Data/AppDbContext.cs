using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Bakcend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Technic> TechnicItems => Set<Technic>();
    public DbSet<TechnicType> TechnicTypes => Set<TechnicType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TechnicType>().HasData(
            new TechnicType { Id = 1, Name = "Системный блок" },
            new TechnicType { Id = 2, Name = "Монитор" },
            new TechnicType { Id = 3, Name = "Ноутбук" },
            new TechnicType { Id = 4, Name = "Принтер/МФУ" },
            new TechnicType { Id = 5, Name = "Сетевое оборудование" });


        modelBuilder.Entity<Technic>().HasIndex(t => t.AccountNumber).IsUnique();
        modelBuilder.Entity<Technic>().HasData(
            new Technic { Id = 1, AccountNumber = "PC001", Name = "ПК офисный №1", TechnicTypeId = 1, RoomNumber = 101 },
            new Technic { Id = 2, AccountNumber = "PC002", Name = "ПК офисный №2", TechnicTypeId = 1, RoomNumber = 102 },
            new Technic { Id = 3, AccountNumber = "MN001", Name = "Монитор Dell 24", TechnicTypeId = 2, RoomNumber = 101 },
            new Technic { Id = 4, AccountNumber = "NB001", Name = "Ноутбук Lenovo", TechnicTypeId = 3, RoomNumber = 205 },
            new Technic { Id = 5, AccountNumber = "PR001", Name = "МФУ Kyocera", TechnicTypeId = 4, RoomNumber = 301 });
    }
}
