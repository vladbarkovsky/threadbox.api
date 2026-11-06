# Introduction 

Threadbox is anonymous imageboard for sharing texts and images.

# Getting Started

1. Install PostgreSQL 18.
1. Clone `threadbox.api` repository.
1. Put the local settings below into user secrets. In Development the host loads them over `appsettings.json`. `ApiSpecificationCredentials:Username` stays in `appsettings.json`.

```json
{
  "ConnectionStrings": {
    "Postgres": "Host=localhost;Database=threadbox_dev;Username=postgres;Password=..."
  },
  "OidcBffClientSecret": "",
  "ApiSpecificationCredentials": {
    "Password": ""
  }
}
```

4. Launch server using 'Threadbox API' profile and Debug configuration.

# API specification and client code generation

API specification is available at https://localhost:5000/. OpenAPI document: https://localhost:5000/api-specification.json. TypeScript API client is generated on the frontend from that document.

## File collections

Send files as a flat `List<IFormFile>` on the form command. Do not use `IFormFile[]`. NSwag treats a file collection as `multipart/form-data` only when the type has an `IFormFile` generic argument. An array has none, so the property is written as a query parameter. The generated client then puts `[object Object]` in the URL instead of appending the files to the form.

NSwag does not generate a command type for an operation that sends `List<IFormFile>`. ApiExplorer exposes each form property as its own parameter, and the client turns each parameter into a method argument. Write that command type by hand.

Do not use `IFormFileCollection` either. NSwag recognizes the type name and puts the property in `multipart/form-data`, but that check does not describe the element type. The OpenAPI 3 schema is generated from the CLR type. `IFormFileCollection` is not generic: it inherits `IReadOnlyList<IFormFile>` and does not declare `GetEnumerator`. NJsonSchema looks for the element type in the type arguments and in `GetEnumerator` on the type itself, finds neither, and writes an empty item schema. An empty schema is `any`, so the generated client types the parameter as `any[]` instead of `FileParameter[]`. `List<IFormFile>` works because its first type argument is `IFormFile`, which NJsonSchema treats as binary.

- https://github.com/RicoSuter/NSwag/blob/v13.20.0/src/NSwag.Generation.AspNetCore/Processors/OperationParameterProcessor.cs#L66-L69
- https://github.com/RicoSuter/NSwag/blob/v13.20.0/src/NSwag.Generation.AspNetCore/Processors/OperationParameterProcessor.cs#L179-L187
- https://github.com/RicoSuter/NSwag/blob/v13.20.0/src/NSwag.Generation.AspNetCore/Processors/OperationParameterProcessor.cs#L319-L324
- https://github.com/RicoSuter/NSwag/blob/v13.20.0/src/NSwag.Generation.AspNetCore/Processors/OperationParameterProcessor.cs#L351-L355
- https://github.com/RicoSuter/NSwag/blob/v13.20.0/src/NSwag.Generation.AspNetCore/Processors/OperationParameterProcessor.cs#L357-L374
- https://github.com/RicoSuter/NJsonSchema/blob/v10.9.0/src/NJsonSchema/Generation/DefaultReflectionService.cs#L278-L290
- https://github.com/RicoSuter/NJsonSchema/blob/v10.9.0/src/NJsonSchema/Generation/JsonSchemaGenerator.cs#L605-L630
- https://github.com/RicoSuter/NJsonSchema/blob/v10.9.0/src/NJsonSchema.CodeGeneration.TypeScript/TypeScriptTypeResolver.cs#L87-L93
- https://github.com/RicoSuter/NSwag/blob/v13.20.0/src/NSwag.CodeGeneration.TypeScript/Models/TypeScriptOperationModel.cs#L173-L175

## Nested file properties

NSwag does not generate a correct specification for nested `IFormFile` properties from form data: a file inside another object, or inside an element of a collection, is missing or misdescribed in the document. But the server still binds nested `IFormFile` properties from form data correctly. Write such client requests by hand.  

- https://github.com/dotnet/aspnetcore/blob/v6.0.36/src/Mvc/Mvc.ApiExplorer/src/DefaultApiDescriptionProvider.cs#L534-L549
- https://github.com/RicoSuter/NSwag/issues/1632
- https://github.com/RicoSuter/NSwag/issues/2822

## Controller actions with multiple sources

An controller action that reads both the route and the request body must put `[FromRoute]` on the action method parameter. On the command, mark the properties as usual: `[FromRoute]` for the route value and `[FromBody]` for the body value.

`[ApiController]` infers a binding source for an action parameter that has none. A complex parameter is inferred as the request body from its type alone, so attributes on the command properties are not consulted. The body source is greedy: it binds the whole object at once and does not walk those properties. The route template still adds its own path parameter, so the generated client asks for the route value twice, once as a method argument and again inside the body.

`[FromRoute]` on the action parameter is not overwritten. The path source is not greedy, so the property attributes are applied.

NSwag does not generate a command type for this operation either. ApiExplorer exposes the route property and the body property as separate parameters, and the client turns each parameter into a method argument. Write that command type by hand.

- https://github.com/RicoSuter/NSwag/blob/v13.20.0/src/NSwag.Generation.AspNetCore/Processors/OperationParameterProcessor.cs#L66-L69
- https://github.com/dotnet/aspnetcore/blob/v6.0.36/src/Mvc/Mvc.Core/src/ApplicationModels/InferParameterBindingInfoConvention.cs#L95-L134
- https://github.com/dotnet/aspnetcore/blob/v6.0.36/src/Mvc/Mvc.Abstractions/src/ModelBinding/BindingSource.cs#L20-L24
- https://github.com/dotnet/aspnetcore/blob/v6.0.36/src/Mvc/Mvc.Abstractions/src/ModelBinding/BindingSource.cs#L66-L70
- https://github.com/dotnet/aspnetcore/blob/v6.0.36/src/Mvc/Mvc.Abstractions/src/ModelBinding/BindingSource.cs#L141-L142
- https://github.com/dotnet/aspnetcore/blob/v6.0.36/src/Mvc/Mvc.ApiExplorer/src/DefaultApiDescriptionProvider.cs#L263-L272
- https://github.com/dotnet/aspnetcore/blob/v6.0.36/src/Mvc/Mvc.ApiExplorer/src/DefaultApiDescriptionProvider.cs#L523-L529

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
