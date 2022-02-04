# Email Sender

Web-сервис для формирования и отправки писем с функцией логирование запросов в БД.

Для отправки сообщения нужно отправить POST-запрос

```json
{
    "subject": "string",
    "body": "string",
    "recipients": [ "string" ]
}
```

по маршруту api/mails.

Данные для отправки писем и авторизации хранятся в appsettings.json.

```json
{
    "Name": "John Doe",
    "Address": "johndoe@unknown.org",
    "Credentials": {
        "Login": "********",
        "Password": "********"
    },
    "Smtp": {
        "Server": {
            "Host": "smtp.unknown.org",
            "Port": "465",
            "UseSsl": true
        }
    }
}
```

Если секция Credentials отсутствует, то предполагается, что используется открытое подключение.

[Скрипт создания экземпляра SQLite хранилища сообщений](./create-sqlite-emails-repository.sql)
