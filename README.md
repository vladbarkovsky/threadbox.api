# Introduction 

Threadbox is anonymous imageboard for sharing texts and images.

# Getting Started

1. Install PostgreSQL 16. Server use 'postgres' user and 'P@ssw0rd' password for connection to 'threadbox_api_dev' database.
2. Install CodeMaid extension for Visual Studio and enable automatic cleanup on save.
3. Clone `threadbox.api` repository. It is important to clone repository in the same directory as the `treadbox.front`, since api-client.ts and api-permissions.ts are generated by server to constant paths.
4. Launch server using 'Threadbox API' profile and Debug configuration.

API specification is available at https://localhost:5000/api.

# Migrations

```
dotnet ef database update -p ThreadboxApi -c ApplicationDbContext
dotnet ef migrations add TemplateMigration -p ThreadboxApi -c ApplicationDbContext -o ORM\Migrations
dotnet ef database update -p ThreadboxApi -c ApplicationDbContext
dotnet ef database update TemplateMigration -p ThreadboxApi -c ApplicationDbContext
dotnet ef migrations remove -p ThreadboxApi -c ApplicationDbContext
dotnet ef database update 0 -p ThreadboxApi -c ApplicationDbContext
```

# Identity

TODO: Identity UI, IdentityServer4, Folders

# Build and Test

TODO: Describe and show how to build your code and run the tests. 

# Contribute

TODO: Explain how other users and developers can contribute to make your code better.
