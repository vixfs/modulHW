# modulHW

## Создание БД

```
CREATE DATABASE MoneyService;

CREATE TABLE users
(
    ID VARCHAR(64) PRIMARY KEY,
    Email VARCHAR(64) UNIQUE,
    PasswordSalt VARCHAR(64) NOT NULL,
    PasswordHash VARCHAR(128) NOT NULL
);

CREATE TABLE accounts
(
    accountNumber BIGINT PRIMARY KEY,
    HolderId VARCHAR(64) NOT NULL REFERENCES users,
    Balance DECIMAL(32,2) NOT NULL CHECK (Balance >= 0) DEFAULT 0
);
```

## Запросы

### Регистрация
POST-запрос https://localhost:5001/users/register
Body (raw, JSON):
```
{
  "Email": "vasya@fail.com",
  "Password": "veryhardpass"
}
```

### Получение токена (авторизация)
POST-запрос https://localhost:5001/users/token
Body (raw, JSON):
```
{
  "Email": "vasya@fail.com",
  "Password": "veryhardpass"
}
```
Получаем токен

### Пополнение баланса (собственного счета)
POST-запрос https://localhost:5001/transactions/refill
Authorization, Bearer token - вставляем токен.
Body (raw, JSON):
```
{
	"amount": 5000
}
```
amount - сумма пополнения.

### Перевод денег
POST-запрос https://localhost:5001/transactions/transfer
Authorization, Bearer token - вставляем токен.
Body (raw, JSON):
```
{
	"RecipientAccountId": 4423638066,
	"amount": 150
}
```
RecipientAccountId - счет получателя (в БД accounts.accountNumber)
amount - сумма перевода
