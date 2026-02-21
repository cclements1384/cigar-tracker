# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
dotnet build                          # Build the project
dotnet run                            # Run the app (https://localhost:7123)
dotnet ef migrations add <Name>       # Create a new EF Core migration
dotnet ef database update             # Apply migrations manually
dotnet ef migrations list             # View migration history
dotnet ef migrations remove           # Undo last migration
dotnet tool install --global dotnet-ef  # Install EF CLI tools if missing
```

## Local Development Configuration

Two secrets are required for local development, stored in .NET User Secrets (never in files):

```bash
dotnet user-secrets set "GoogleAuthClientId" "<your-client-id>"
dotnet user-secrets set "GoogleAuthClientSecret" "<your-client-secret>"
```

The Azure SQL connection string goes in `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "CigarTrackerDb": "Server=tcp:<server>.database.windows.net,1433;..."
  }
}
```

## Architecture

**ASP.NET Core 8 Blazor Server** app using Interactive Server rendering throughout.

- `Program.cs` — Service registration and middleware pipeline. Registers Google OIDC auth, EF Core (Azure SQL), `CigarService` (scoped), and `UpcService` (via `AddHttpClient`). Also maps `/login` and `/logout` endpoints.
- `Models/` — `Cigar` (the core entity) and `UpcProduct` (API response DTO).
- `Data/CigarTrackerDbContext.cs` — EF Core DbContext with a single `Cigars` DbSet and model configuration.
- `Services/CigarService.cs` — CRUD operations against the database (injected into Blazor pages).
- `Services/UpcService.cs` — HTTP client calling `api.upcitemdb.com` to look up product info by UPC barcode.
- `Components/Pages/` — Blazor pages. `CigarLog.razor` and `UpcLookup.razor` both require `[Authorize]` and use `@rendermode InteractiveServer`.
- `Migrations/` — EF Core migrations. Migrations are applied automatically on startup.

**Authentication flow**: Google OIDC → cookie auth. The `[Authorize]` attribute on pages redirects unauthenticated users to Google login. Logout is handled via a POST to `/logout`.

**Production secrets** (connection string, Google credentials) are stored in Azure Key Vault (`kv-cigar-tracker`). The app loads Key Vault config automatically in non-development environments using `DefaultAzureCredential`.
