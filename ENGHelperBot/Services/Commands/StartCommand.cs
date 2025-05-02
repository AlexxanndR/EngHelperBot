using ENGHelperBot.Services.Repositories.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ENGHelperBot.Services.Command;

public class StartCommand(IServiceScopeFactory scopeFactory) : ICommandHandler
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public async Task<Message> HandleAsync(ITelegramBotClient bot, Update update)
    {
        const string usage = """
            <b>🌟 Welcome to your personal English helper! 🌟</b>  

            Этот бот поможет вам легко учить английские слова.  

            <b><u>📖 Главное меню:</u></b>  

            <b>📚 Мои словари</b> — здесь живут все ваши подборки слов.  
            Можно создавать новые (например, «Для путешествий» или «IT-термины»), удалять старые или пересматривать коллекцию.  

            <b>➕ Добавить слово</b> — пополняйте словарный запас!  
            Просто введите слово — бот запомнит его в выбранном словаре.  

            <b>📝 Пройти тест</b> — проверьте, как хорошо вы запомнили слова. Бот будет задавать вопросы, а вы выбирать ответы.  
        """;

        using var scope = _scopeFactory.CreateScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

        var newUser = new Data.Entities.User() { Id = update.Message!.Chat.Id, Username = update.Message.Chat.Username };
        await userRepository.CreateAsync(newUser, u => u.Id == newUser.Id);

        return await bot.SendMessage(update.Message.Chat, usage, parseMode: ParseMode.Html, replyMarkup: new string[][]
        {
            [BotCommandTexts.MyDictionaries], [BotCommandTexts.AddWord], [BotCommandTexts.TakeTest]
        });
    }
}
