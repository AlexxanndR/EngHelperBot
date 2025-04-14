using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Net;

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

    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(options =>
        {
            options.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                var exception = context.Features.Get<IExceptionHandlerFeature>();
                if (exception != null)
                {
                    var message = $"{exception.Error.Message}";
                    await context.Response.WriteAsync(message).ConfigureAwait(false);
                }
            });
        });
    }
}
