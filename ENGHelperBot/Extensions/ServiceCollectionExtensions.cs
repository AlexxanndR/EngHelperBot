using Microsoft.EntityFrameworkCore;

namespace ENGHelperBot.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddBotConfiguration(
        this IServiceCollection services, 
        string botTokenVar = "BOT_TOKEN",
        string webhookUrlVar = "WEBHOOK_URL",
        string secretTokenVar = "SECRET_TOKEN") 
    {
        var botToken = Environment.GetEnvironmentVariable(botTokenVar)
            ?? throw new InvalidOperationException($"{botTokenVar} environment variable is not set.");

        var webhookUrl = Environment.GetEnvironmentVariable(webhookUrlVar)
            ?? throw new InvalidOperationException($"{webhookUrlVar} environment variable is not set.");

        var secretToken = Environment.GetEnvironmentVariable(secretTokenVar)
            ?? throw new InvalidOperationException($"{secretTokenVar} environment variable is not set.");

        if (!Uri.TryCreate(webhookUrl, UriKind.Absolute, out var parsedUri))
            throw new InvalidOperationException($"{webhookUrlVar} contains invalid URI format");

        services.AddSingleton(new BotConfiguration
        {
            BotToken = botToken,
            BotWebhookUrl = parsedUri,
            SecretToken = secretToken
        });
    }

    public static void ConfigureDatabaseConnection(
        this IServiceCollection services,
        string dbHostVar = "DB_HOST",
        string dbPortVar = "DB_PORT",
        string dbNameVar = "DB_NAME",
        string dbUsernameVar = "DB_USERNAME",
        string dbPasswordVar = "DB_PASSWORD")
    {
        var host = Environment.GetEnvironmentVariable(dbHostVar) ??
            throw new NullReferenceException($"{dbHostVar} environment variable is not set.");
        var port = Environment.GetEnvironmentVariable(dbPortVar) ??
            throw new NullReferenceException($"{dbPortVar} environment variable is not set.");
        var name = Environment.GetEnvironmentVariable(dbNameVar) ??
            throw new NullReferenceException($"{dbNameVar} environment variable is not set.");
        var username = Environment.GetEnvironmentVariable(dbUsernameVar) ??
            throw new NullReferenceException($"{dbUsernameVar} environment variable is not set.");
        var password = Environment.GetEnvironmentVariable(dbPasswordVar) ??
            throw new NullReferenceException($"{dbPasswordVar} environment variable is not set.");

        services.AddDbContextFactory<AppDbContext>(options =>
            options.UseNpgsql($"User ID={username};Password={password};Host={host};Port={port};Database={name};"));
    }
}
