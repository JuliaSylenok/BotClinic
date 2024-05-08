# Практична робота №2
## ПАРАЛЕЛЬНІ ЗАДАЧИ ТА МЕРЕЖЕВА ВЗАЄМОДІЯ
## ПОСТАНОВКА ЗАВДАННЯ
### Завдання
Розробити програмний чат-бот для Telegram, використовуючи методи та засоби паралельного програмування платформи .Net. Бот повинен мати базовий набір функціональних можливостей для взаємодії з користувачами, включаючи відправлення повідомлень, обробку команд та використання асинхронних методів для ефективної роботи. Складність і функціональність бота визначається самостійно (див. табл. 2.1) і вибирається по одному з наступних напрямків:
- Предметна область відповідає курсовому проекту з ООП (допускається реалізація як самостійної програми чат-бота, так і з інтеграцією безпосередньо у курсовий проект);
- Предметна область визначається на основі даних з одного або декількох відкритих та загальнодоступних Web API. Чат-бот фактично організує інтерфейс користувача в Telegram для інформації, яка отримується з вибраного Web API, наприклад, за допомогою методів класу HttpClient;
- Інші варіанти предметної галузі (після обговорення з викладачем).
### Порядок виконання роботи
1. Telegram знайти спеціальний вбудований бот BotFather, в якому за допомогою команди /newbot створити власний чат-бот та отримати його API-Token. Зверніть увагу, що API-Token є секретом та не підлягає розголошенню!
2. Створити новий проект у середовищі розробки Visual Studio чи іншій IDE для .Net . Тип проекту може бути будь-яким.
3. Налаштувати проект для роботи з Telegram API (наприклад, через менеджер NuGet встановити пакет Telegram.Bot ).
4. Створити у програмі екземпляр класу TelegramBotClient з отриманим API-Token.
5. У створеному у п.4. об'єкті реалізувати та підключити обробники помилок та повідомлень бота.
6. Реалізувати код для обробки команд (наприклад, /start, /help тощо на власний розсуд), які керують логікою роботи чат-бота.
7. Реалізувати код надсилання повідомлень користувачу та обробки повідомлень, які надходять від користувача.
8. Використовувати асинхронні методи для мережної взаємодії та виконання тривалих операцій (наприклад, запити до стороннього API).
9. Протестувати бота на локальній машині та/або розгорнутого на хостингу.
## ВИКОНАННЯ РОБОТИ
### Варіант 19
Предметна область відповідає курсовому проекту з ООП - "Приватна клініка"
### Результат створення бота
Створено бота приватної клініки (ClinicBot) за допомогою вбудованого бота BotFather.

Що може бот ClinicBot:
-	Надає можливість користувачу переглянути всі послуги, що надаються клінікою;
 ![image](https://github.com/JuliaSylenok/BotClinic/assets/149322465/163c4560-6e24-42f7-a751-3c71ca0e0bca)

-	Надає можливість шукати послуги відповідно до їх категорії;
 ![image](https://github.com/JuliaSylenok/BotClinic/assets/149322465/dec520ed-e383-4c45-ba3c-4332bd663caf)

-	Надає можливість переглянути коротку загальну інформацію та фото клініки.
 ![image](https://github.com/JuliaSylenok/BotClinic/assets/149322465/8b8c8e69-591a-411a-a224-34dab9de3472)

## ВИСНОВОК

В результаті практичної роботи було створено за допомогою BotFather бота приватної клініки - ClinicBot, що реалізує функції перегляду інформації про клініку, послуги та реалізує пошук послуг по їх категоріям.
