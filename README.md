# Budget Tracker API

[![CI](https://github.com/mackyten/budget-tracker-api/actions/workflows/ci.yml/badge.svg)](https://github.com/mackyten/budget-tracker-api/actions/workflows/ci.yml)

.NET 8 Web API behind the [Budget Tracker web app](https://github.com/mackyten/budget-tracker-web).

## Tech stack

- ASP.NET Core, MediatR (CQRS), FluentValidation, AutoMapper
- Entity Framework Core with SQL Server
- ASP.NET Identity with JWT bearer authentication
- Swagger, GitHub Actions deploy to Azure

## Solution layout

| Project | Role |
|---|---|
| `BT.API` | Controllers, configuration, filters |
| `BT.APPLICATION` | Commands, queries, handlers |
| `BT.DOMAIN` | Entities and enums |
| `BT.PERSISTENCE` | DbContext and migrations |
| `BT.SERVICES` | Email, data seeding, Supabase and account services |

## Endpoints

- `POST /api/auth/register`, `/login`, `/logout`
- `POST /api/income/create`, `GET /api/income/get-list`

## Getting started

```bash
dotnet restore
dotnet ef database update --project BT.PERSISTENCE --startup-project BT.API
dotnet run --project BT.API
```

Configure the SQL Server connection string and JWT key through environment variables or user secrets. Do not commit them.

## Tests

```bash
dotnet test BT.API.sln
```

xUnit tests run the MediatR handlers against EF Core's in-memory provider: creating an income with its bank, cash and deduction rows, the audit fields stamped from the signed-in user, pagination metadata, and the not-found path.
