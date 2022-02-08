using Telegram.Bot;

namespace DiaB.Core.Web.Helpers
{
    public static class TelegramHelper
    {
        public static TelegramBotClient InitializeTelegramBotClient(string token)
        {
            return new TelegramBotClient(token);
        }
    }
}
