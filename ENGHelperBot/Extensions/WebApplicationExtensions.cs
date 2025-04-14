using Microsoft.EntityFrameworkCore;

namespace ENGHelperBot.Extensions;

public static class WebApplicationExtensions
{
    public async static void MigrateAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any()) await dbContext.Database.MigrateAsync();
    }
}
