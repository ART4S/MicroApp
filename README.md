# MicroShop

Целью проекта было изучение и практическое применение концепций из книги [.NET Microservices: Architecture for Containerized .NET Applications](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/)

За основу была взята бизнес модель представленного в книге проекта [eShopOnContainers](https://github.com/dotnet-architecture/eShopOnContainers)

## Сервисы

Проект разделен на несколько модулей (сервисов) в зависимости от ответственности:

1. **Identity** 
	
	Или сервер аутентификации (OAuth).  Сервис хранит в себе данные пользователей и их роли в системе. Позволяет произвести логин пользователя в системе (логаут и регистрация не 	реализованы) и поддерживает Authorization Code Flow для получаения токена пользователя, с которым в последствии можно авторизоваться в любом сервисе.

2. **Catalog**

	Работа с товарами: их фильтрация, создание, редактирование и удаление

3. **Basket**

	Простой сервис хранения корзины пользователя.

4. **Ordering**

	Сервис работы с заказами. В основу работы сервиса входит оформление заказа пользователя и его дальнейшая обработка.

5. **Ordering-SignalR**

	Real-time уведомления о смене статуса заказа.

6. **WebHealthMonitor**

	Используется для контроля за состоянием сервисов. Предоставляет актуальные данные по доступности каждого сервиса и их ресурсов с помощью HealthChecks.


## Модель
Структура данных проекта выглядит следующим образом:

**Catalog**

![Catalog ERD](https://github.com/ART4S/MicroShop/blob/master/Resources/Catalog%20ERD.PNG)

**Basket**

![Basket ERD](https://github.com/ART4S/MicroShop/blob/master/Resources/Basket%20ERD.PNG)

**Ordering**

![Ordering ERD](https://github.com/ART4S/MicroShop/blob/master/Resources/Ordering%20ERD.PNG)

**Identity**

![Identity ERD](https://github.com/ART4S/MicroShop/blob/master/Resources/Identity%20ERD.PNG)

Отдельно стоит упомянуть, что внутренняя структура данных сервиса построенна на основе [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-6.0&tabs=visual-studio) (не путать с [IdentityServer4](https://identityserver4.readthedocs.io/en/latest/)). Т.е на представленной схеме объекты являются прототипами, на основе которых ASP.NET Core Identity построила свою модель с таблицами и связями.

## Хранение данных

**Catalog**

В основе хранения данных сервиса лежит mongodb т.к допускается чтобы данные были не согласованными и связи между коллекциями едва ли могут меняться.

**Basket**

Для сервиса был выбран Redis т.к сама бизнес модель не предуматривает множества таблиц и их связей, простое хранение пар ключ-значение вполне устраивает.

**Identity, Ordering**

Для остальных сервисов был использован mssql

## Контейнеризация

Работоспособность всех сервисов можно протестировать в Docker, для этого в каждом проекте присутствует отдельный Dockerfile, а для централизованной сборки и запуска – проект docker-compose.
Для удобства конфигурирования почти все настройки сервисов вынесены из appsettings.json в файл docker-compose.override.yml

![Docker](https://github.com/ART4S/MicroShop/blob/master/Resources/Docker.PNG)

## Отправка событий

Т.к никто не застрахован от сбоев в сервисах, так или иначе может возникнуть ситуация когда данные сохранены, но событие об изменении этих данных не отправлено в очередь сообщений. Для устранения этой проблемы был использован предложенный в книге Outbox pattern.
1.	В базе данных завоидится отдельная таблица для исходящий событий.
2.	Сохраняем измененные данные сервиса и событие об их изменении в одной транзакции (Шаг 1)
3.	При старте сервера запускается задача, которая будет периодически извлекать из бд список не отправленных событий и отправлять их в порядке времении их создания (Шаг 2, 3)
4.	Очередь направляет поступившие событие на подписанные сервисы (Шаг 4)

При таком способе отправки событий так же могут возникнуть сбои в результате которых событие будет отправлено в очередь, но пометка о том что событие отправлено не будет занесено в локальную бд и таким образом в конечном итоге событие может быть отправлено дважды.

Для избежания таких ситуаций были сделаны следующие вещи:

1.	Идемпотентные команды (многократное выполнение приведет к одинаковому результату).
2.	Для команд, которые в силу обстоятельств сложно сделать идемпотентными был сделан специальный враппер IdempotentCommand, куда заносится идентификатор события и команда на исполнение. При отправке такой команды идёт проверка на присутствие в бд идентификатора события (Шаг 5), если присутствут – вложенная команда не выполняется, если отсутствует – идентификатор вносится в базу данных (Шаг 6) и вложенная команда выполняется.

Описанная схема отправки событий вместе с outbox pattern выглядит следующим образом:

![Idempotency](https://github.com/ART4S/MicroShop/blob/master/Resources/Idempotency.PNG)

Шаг 7:

![Idempotency](https://github.com/ART4S/MicroShop/blob/master/Resources/Idempotency(1).PNG)

## Тестирование API

Для проверки работоспособности приложения был подготовлен набор запросов и минимальный набор тестов через Postman. Достаточно сделать импорт следующих файлов:
```
MicroShop.postman_collection.json
MicroShop.postman_environment.json
```
![Postman](https://github.com/ART4S/MicroShop/blob/master/Resources/Postman.PNG)

Тестирование корзины по grpc может быть выполнено с помощью утилиты https://github.com/fullstorydev/grpcui. Для запуска достаточно ввести в командную строку следующее:
```
grpcui -plaintext localhost:5003
```

![grpcui](https://github.com/ART4S/MicroShop/blob/master/Resources/grpcui.PNG)

# Developer Notes

### Создание миграций

**Catalog**

*CatalogDbContext*: dotnet ef migrations add Catalog_Initial -s ./Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj -p ./Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj -c CatalogDbContext -o DataAccess/Catalog/Migrations

*IntegrationDbContext*: dotnet ef migrations add Integration_Initial -s ./Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj -p ./Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj -c IntegrationDbContext -o DataAccess/Integration/Migrations

**Ordering**

*OrderingDbContext*: dotnet ef migrations add Ordering_Initial -s ./Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj -p ./Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj -c OrderingDbContext -o DataAccess/Ordering/Migrations

*IntegrationDbContext*: dotnet ef migrations add Integration_Initial -s ./Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj -p ./Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj -c IntegrationDbContext -o DataAccess/Integration/Migrations

*IdempotencyDbContext*: dotnet ef migrations add Idempotency_Initial -s ./Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj -p ./Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj -c IdempotencyDbContext -o DataAccess/Idempotency/Migrations

**Payment**

*IntegrationDbContext*: dotnet ef migrations add Integration_Initial -s ./Services/Payment/Payment.API/Payment.API.csproj -p ./Services/Payment/Payment.API/Payment.API.csproj -c IntegrationDbContext -o DataAccess/Integration/Migrations


**Identity**

*PersistedGrantDbContext*: dotnet ef migrations add PersistedGrant_Initial -s ./Services/Identity/Identity.API/Identity.API.csproj -p ./Services/Identity/Identity.API/Identity.API.csproj -c PersistedGrantDbContext -o DataAccess/Migrations/PersistedGrant

*ConfigurationDbContext*: dotnet ef migrations add Configuration_Initial  -s ./Services/Identity/Identity.API/Identity.API.csproj -p ./Services/Identity/Identity.API/Identity.API.csproj -c ConfigurationDbContext -o DataAccess/Migrations/Configuration

*AppDbContext*: dotnet ef migrations add App_Initial -s ./Services/Identity/Identity.API/Identity.API.csproj -p ./Services/Identity/Identity.API/Identity.API.csproj -c AppDbContext -o DataAccess/Migrations/App
