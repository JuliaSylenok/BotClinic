using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


public class ClinicService
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
}

public class Program
{
    private static List<ClinicService> clinicServices = new List<ClinicService>
    {
        new ClinicService { Title = "Прийом лікаря", Description = "Прийом лікаря загальної практики", Category = "Прийоми", Price = 100 },
        new ClinicService { Title = "Консультація спеціаліста", Description = "Консультація фахівця за вибором", Category = "Консультації", Price = 150 },
        new ClinicService { Title = "УЗД органів черевної порожнини", Description = "Ультразвукове дослідження органів черевної порожнини", Category = "Діагностика", Price = 200 },
        new ClinicService { Title = "Аналіз крові", Description = "Загальний аналіз крові", Category = "Діагностика", Price = 80 },
        new ClinicService { Title = "Вакцинація", Description = "Введення вакцини за вибором", Category = "Профілактика", Price = 120 },
        new ClinicService { Title = "Загальне обстеження зубів", Description = "Загальне обстеження зубів та поради стосовно їх догляду", Category = "Стоматологія", Price = 90 },
        new ClinicService { Title = "Рентген зубів", Description = "Рентгенівське дослідження зубів", Category = "Стоматологія", Price = 150 },
        new ClinicService { Title = "Чистка зубів", Description = "Професійна чистка зубів", Category = "Стоматологія", Price = 70 }
    };

    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        var botClient = new TelegramBotClient("7098728661:AAFsJ9vkHsc-0gVUmbmQti0FCLZOrM0HJTA");

        using CancellationTokenSource cts = new();

        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        botClient.StartReceiving(
            updateHandler: async (botClient, update, cancellationToken) => await HandleUpdateAsync(botClient, update, cancellationToken),
            pollingErrorHandler: async (botClient, exception, cancellationToken) => await HandlePollingErrorAsync(botClient, exception, cancellationToken),
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );

        var me = await botClient.GetMeAsync();

        Console.WriteLine($"Start listening for @{me.Username}");
        Console.ReadLine();

        cts.Cancel();
    }

    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message)
            return;

        if (message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;

        Console.WriteLine($"Received a '{messageText}' message in chat {chatId} from {message?.Chat.FirstName}.");

        var command = messageText.Split(' ').First().ToLower();

        if (command == "/start")
        {
            await SendMainMenuAsync(botClient, chatId, cancellationToken);
        }

        else if (command == "/services" || messageText == "Переглянути послуги")
        {
            await SendAllServicesAsync(botClient, chatId, cancellationToken);
        }

        else if (command == "/category" || messageText == "Пошук послуг за категорією")
        {
            await SendCategoriesAsync(botClient, chatId, cancellationToken);
        }
        else if (command == "/information" || messageText == "Переглянути інформацію про клініку")
        {
            await SendClinicInformationAsync(botClient, chatId, cancellationToken);
        }


        else if (clinicServices.Any(s => s.Category.ToLower() == messageText.ToLower().Trim()))
        {
            var category = messageText.ToLower().Trim();
            Console.WriteLine($"Пошук послуг у категорії: '{category}'");
            await SendServicesByCategoryAsync(botClient, chatId, category, cancellationToken);
            Console.WriteLine("Послуги надіслано.");
        }
    }
    private static async Task SendClinicInformationAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
    {
        string clinicInfo = "Це наша приватна клініка. Ми пропонуємо широкий спектр медичних послуг для наших пацієнтів.";
        await botClient.SendPhotoAsync(
            chatId: chatId,
            photo: InputFile.FromUri("https://cuhcpc.ie/wp-content/uploads/2021/01/Consultants-Private-Clinic-Main-Building.jpg"),

            caption: clinicInfo,
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken);
    }



    private static async Task SendMainMenuAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
    {
        var keyboard = new ReplyKeyboardMarkup(new[]
        {
        new[]
        {
            new KeyboardButton("Переглянути послуги"),
            new KeyboardButton("Пошук послуг за категорією")
        },
        new[]
        {
            new KeyboardButton("Переглянути інформацію про клініку")
        }
    });

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Меню:",
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);
    }

    private static async Task SendCategoriesAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
    {
        var categories = clinicServices.Select(s => s.Category).Distinct().ToList();
        var keyboardButtons = categories.Select(c => new KeyboardButton(c)).ToArray();
        var keyboard = new ReplyKeyboardMarkup(keyboardButtons);

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Оберіть категорію:",
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);
    }

    private static async Task SendServicesByCategoryAsync(ITelegramBotClient botClient, long chatId, string category, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Пошук послуг у категорії: '{category}'");

        var servicesInCategory = clinicServices.Where(s => s.Category.ToLower() == category.ToLower()).ToList();
        if (servicesInCategory.Any())
        {
            var messageText = $"Послуги у категорії '{category}':\n\n";
            foreach (var service in servicesInCategory)
            {
                messageText += $"*{service.Title}*\n_{service.Description}_\nЦіна: {service.Price} грн\n\n";
            }
            Console.WriteLine("Надсилаю послуги");
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: messageText,
                cancellationToken: cancellationToken,
                parseMode: ParseMode.MarkdownV2);
            Console.WriteLine("Послуги надіслано");

            await SendMainMenuAsync(botClient, chatId, cancellationToken);
        }
        else
        {
            Console.WriteLine($"Послуги у категорії '{category}' не знайдено");
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"Послуги у категорії '{category}' не знайдено",
                cancellationToken: cancellationToken,
                parseMode: ParseMode.MarkdownV2);

            await SendMainMenuAsync(botClient, chatId, cancellationToken);
        }
    }

    private static async Task SendAllServicesAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
    {
        var messageText = "Ось усі доступні послуги:\n\n";
        foreach (var service in clinicServices)
        {
            messageText += $"*{service.Title}*\n_{service.Description}_\nЦіна: {service.Price} грн\n\n";
        }

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: messageText,
            cancellationToken: cancellationToken,
            parseMode: ParseMode.MarkdownV2);

        Console.WriteLine("Усі послуги надіслано");
    }

    private static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}

