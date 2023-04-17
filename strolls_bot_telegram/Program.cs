using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using Newtonsoft.Json;
using strolls_bot_telegram.JsonObjects;
using Telegram.Bot.Types.ReplyMarkups;

namespace strolls_bot_telegram
{

    class Program
    {
        static ITelegramBotClient bot = new TelegramBotClient("5639704722:AAFb1n2eJS3x8lLBWZF8CnWRRFUmy21GMuE");

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var client = new HttpClient();

            // Если появилось новое сообщение
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;

                // Ответ на /start
                if (message?.Text?.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Приветствую. Введите IMEI номер:");
                    return;
                }

                // Ответ на IMEI номер(минимальная валидация)
                if (message?.Text?.Length == 15 && long.TryParse(message.Text, out long imeiNumber))
                {
                    DataStorage.Imei = message.Text;
                    var payload = new Imei { Value = message.Text };
                    var json = JsonConvert.SerializeObject(payload);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://localhost:5000/api/strolls/info", content); // localhost можно заменить на ссылку от тунелля через ngrok(работает, однако лично у меня не всегда, не знаю с чем это связанно)

                    var result = await response.Content.ReadAsStringAsync();

                    var strollsData = JsonConvert.DeserializeObject<MainJson>(result);

                    var totalStrollsCount = strollsData.StrollsData.TotalStrolls;
                    var totalStrollsTime = strollsData.StrollsData.TotalTime;
                    var totalStrollsDistance = strollsData.StrollsData.TotalMeters;


                    // Создание инлайн-кнопки
                    var top10Button = new InlineKeyboardButton("ТОП-10 прогулок")
                    {
                        CallbackData = "top10",
                    };

                    var keyboard = new InlineKeyboardMarkup(new[] { new[] { top10Button } });

                    await botClient.SendTextMessageAsync(message.Chat,
                        $"Информация по IMEI {message.Text}:\nВсего прогулок: {totalStrollsCount}\nВсего времени, мин.: {Math.Round(totalStrollsTime)}\nОбщая дистанция, м: {Math.Round(totalStrollsDistance)}",
                        replyMarkup: keyboard);

                    return;
                }
                // Ответ на все другие сообщения
                await botClient.SendTextMessageAsync(message.Chat, "Введите корректный IMEI номер:");
            }

            // Если была нажата инлайн-кнопка
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {
                var callbackQuery = update.CallbackQuery;
                if (callbackQuery?.Data == "top10")
                {
                    var payload = new Imei { Value = DataStorage.Imei };
                    var json = JsonConvert.SerializeObject(payload);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://localhost:5000/api/strolls/info", content); // localhost можно заменить на ссылку от тунелля через ngrok(работает, однако лично у меня не всегда, не знаю с чем это связанно)

                    var result = await response.Content.ReadAsStringAsync();

                    var strollsData = JsonConvert.DeserializeObject<MainJson>(result);

                    var topStrolls = strollsData.TopStrolls;

                    // Петля для создания таблицы-топа прогулок
                    string table = "";

                    table += "Время | Расстояние\n";

                    for (int i = 0; i < topStrolls.Count; i++)
                    {
                        double time = topStrolls[i].TotalTime;
                        double distance = topStrolls[i].TotalDistance;


                        string row = $"{Math.Round(time),8:N2} | {Math.Round(distance),10:N2}\n";

                        table += row;
                    }

                    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat, table);
                    return;
                }
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(JsonConvert.SerializeObject(exception));
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Bot started " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
    }
}