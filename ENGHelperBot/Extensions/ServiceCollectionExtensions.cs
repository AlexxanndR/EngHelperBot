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
}
