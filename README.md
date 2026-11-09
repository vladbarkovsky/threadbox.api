# Introduction 

Threadbox is anonymous imageboard for sharing texts and images.

# Getting Started

1. Install PostgreSQL 18.
1. Clone `threadbox.api` repository.
1. Put the local settings below into user secrets. In Development the host loads them over `appsettings.json`.

```json
{
  "ConnectionStrings": {
    "Postgres": "Host=localhost;Database=threadbox_dev;Username=postgres;Password=..."
  },
  "OidcBffClientSecret": "...",
}
```

4. Launch server using 'Threadbox API' profile and Debug configuration.

# API specification and client code generation

API specification is available at <https://localhost:5000/>. OpenAPI document: <https://localhost:5000/api-specification.json>. TypeScript API client is generated on the frontend from that document.

A request must use only one data source: the route, the query string, a JSON body, or form data. This constraint are intended to support a single request DTO that aggregates all request parameters, while accommodating NSwag limitations.

## File collections

Send files as a flat `List<IFormFile>` on the form command. Do not use `IFormFile[]`. NSwag treats a file collection as `multipart/form-data` only when the type has an `IFormFile` generic argument. An array has none, so the property is written as a query parameter. The generated client then puts `[object Object]` in the URL instead of appending the files to the form.

NSwag does not generate a command type for an operation that sends `List<IFormFile>`. ApiExplorer exposes each form property as its own parameter, and the client turns each parameter into a method argument. Write that command type by hand.

Do not use `IFormFileCollection` either. The generated client types such parameters as `any[]` instead of `FileParameter[]`.

An empty file `FileParameter[]` array on the client adds no parts to the `multipart/form-data` body. Model binding then leaves the `List<IFormFile>` property `null`. This should be taken into account when defining DTOs. It is recommended to initialize `List<IFormFile>` properties at declaration to avoid null reference exceptions.

## Nested properties and multipart/form-data

NSwag does not generate a correct specification for nested properties from form data. Write such client requests by hand.

- <https://github.com/RicoSuter/NSwag/issues/1632>
- <https://github.com/RicoSuter/NSwag/issues/2822>

# Database migrations

Run these commands from the `threadbox.api` directory.

Apply every migration that is not in the database yet. Run this after cloning the repository, after pulling new migrations, and after adding one.

```
dotnet ef database update --project ThreadboxApi --context ApplicationDbContext
```

Add a migration after changing the entity model. Replace `MigrationName` with a name that describes the change.

```
dotnet ef migrations add MigrationName --project ThreadboxApi --context ApplicationDbContext --output-dir ORM\Migrations
```

Move the database to one chosen migration, including back to an older one. Replace `MigrationName` with the target migration. Migrations added after that target are no longer applied.

```
dotnet ef database update MigrationName --project ThreadboxApi --context ApplicationDbContext
```

Remove the last migration from the project. Do this when that migration was created by mistake. If the database is already on it, move the database to the previous migration first.

```
dotnet ef migrations remove --project ThreadboxApi --context ApplicationDbContext
```

Remove every migration from the database and return it to the state before the first migration. `0` is that empty state. Use this only on a local database you are willing to recreate.

```
dotnet ef database update 0 --project ThreadboxApi --context ApplicationDbContext
```

# Identity

TODO: Identity UI, IdentityServer4, Folders

# Build and Test

TODO: Describe and show how to build your code and run the tests. 

# Contribute

TODO: Explain how other users and developers can contribute to make your code better.
