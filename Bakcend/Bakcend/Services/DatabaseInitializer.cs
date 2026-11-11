using Bakcend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class DatabaseInitializer(IServiceScopeFactory scopeFactory) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await db.Database.MigrateAsync(cancellationToken);

        if (!await db.TechnicTypes.AnyAsync(cancellationToken))
        {
            db.TechnicTypes.AddRange(
                new TechnicType { Name = "Системный блок" },
                new TechnicType { Name = "Монитор" },
                new TechnicType { Name = "Ноутбук" },
                new TechnicType { Name = "Принтер/МФУ" },
                new TechnicType { Name = "Сетевое оборудование" });

            await db.SaveChangesAsync(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
