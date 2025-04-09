using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot;
using ENGHelperBot.Services;
using Telegram.Bot.Polling;

namespace ENGHelperBot.Controllers;

[ApiController]
[Route("[controller]")]
public class BotController(BotConfiguration botConfig) : ControllerBase
{
    private readonly BotConfiguration _botConfig = botConfig;

    [HttpGet("setWebhook")]
    public async Task<string> SetWebHook([FromServices] ITelegramBotClient bot, CancellationToken ct)
    {
        var webhookUrl = _botConfig.BotWebhookUrl.AbsoluteUri;
        await bot.SetWebhook(webhookUrl, allowedUpdates: [], secretToken: _botConfig.SecretToken, cancellationToken: ct);
        return $"Webhook set to {webhookUrl}";
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update, [FromServices] ITelegramBotClient bot, [FromServices] UpdateHandler handleUpdateService, CancellationToken ct)
    {
        //if (Request.Headers["X-Telegram-Bot-Api-Secret-Token"] != Config.Value.SecretToken)
        //    return Forbid();

        try
        {
            await handleUpdateService.HandleUpdateAsync(bot, update, ct);
        }
        catch (Exception exception)
        {
            await handleUpdateService.HandleErrorAsync(bot, exception, HandleErrorSource.HandleUpdateError, ct);
        }

        return Ok();
    }
}
