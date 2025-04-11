using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ENGHelperBot.Services.Command;

public class StartCommand : ICommandHandler
{
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

        return await bot.SendMessage(update.Message!.Chat, usage, parseMode: ParseMode.Html, replyMarkup: new string[][]
        {
            ["📚 Мои словари"], ["➕ Добавить слово"], ["📝 Пройти тест"]
        });
    }
}
