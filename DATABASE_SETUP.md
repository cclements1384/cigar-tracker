# Database Setup Guide

## Overview

The Cigar Tracker application now uses PostgreSQL with Entity Framework Core for data persistence. The database is hosted on Azure with a private endpoint.

## Connection String Configuration

### Development

The development connection string is in `appsettings.Development.json`:

```json
"ConnectionStrings": {
  "CigarTrackerDb": "Server=localhost;Port=5432;Database=cigar_tracker_dev;User Id=postgres;Password=password;"
}
```

Update this with your local PostgreSQL credentials.

### Production (Azure)

In production, the connection string is retrieved from **Azure Key Vault** under the key `CigarTrackerDb`. 

To configure:
1. Set the `KeyVaultName` in `appsettings.json` (e.g., `"kv-cigar-tracker"`)
2. Add the PostgreSQL connection string to Azure Key Vault with the name `CigarTrackerDb`

The application will automatically use the Azure Key Vault configuration in non-development environments.

## Migrations

### Creating Migrations

When you modify the database models, create a migration:

```bash
dotnet ef migrations add <MigrationName>
```

Example:
```bash
dotnet ef migrations add AddCigarColumns
```

### Applying Migrations

Migrations are applied automatically on application startup. To manually apply:

```bash
dotnet ef database update
```

### Viewing Migration History

```bash
dotnet ef migrations list
```

## Initial Migration

The initial migration `InitialCreate` creates the `Cigars` table with the following schema:

- `Id` (int, Primary Key, Auto-generated)
- `Brand` (varchar(255), Required)
- `Size` (varchar(100), Required)
- `Rating` (int, Required)
- `DateSmoked` (timestamp, Required)
- `Notes` (varchar(1000), Optional)

## PostgreSQL Connection Notes

- **Host**: Azure-hosted PostgreSQL (private endpoint)
- **Port**: 5432
- **SSL**: Verify that your connection string includes SSL requirements if needed
- **Authentication**: Use connection string or MSI (Managed Identity) in Azure

## Troubleshooting

### Connection Errors

If you get connection errors:
1. Verify the PostgreSQL server is running
2. Check the connection string in `appsettings.json` or `appsettings.Development.json`
3. Ensure the database user has permissions to create tables (for migrations)
4. For Azure: Verify the private endpoint is accessible from your network

### Migration Issues

If a migration fails:
1. Check the error message in the application logs
2. Manually review the migration file in the `Migrations/` folder
3. Roll back if necessary: `dotnet ef migrations remove`
4. Re-create after fixing the model

## Entity Framework CLI Tools

Ensure you have the EF Core tools installed:

```bash
dotnet tool install --global dotnet-ef
```

Or update if already installed:

```bash
dotnet tool update --global dotnet-ef
```
