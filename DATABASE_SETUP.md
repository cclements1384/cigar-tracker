# Database Setup Guide

## Overview

The Cigar Tracker application now uses **Azure SQL** with Entity Framework Core for data persistence.

## Connection String Configuration

### Development

The development connection string is in `appsettings.Development.json`:

```json
"ConnectionStrings": {
  "CigarTrackerDb": "Server=tcp:YOUR_AZURE_SQL_SERVER.database.windows.net,1433;Initial Catalog=cigar_tracker_dev;Persist Security Info=False;User ID=YOUR_USERNAME;Password=YOUR_PASSWORD;Encrypt=True;Connection Timeout=30;"
}
```

Update the placeholders:
- `YOUR_AZURE_SQL_SERVER`: Your Azure SQL server name (e.g., `myserver`)
- `YOUR_USERNAME`: Your SQL login (e.g., `sqladmin`)
- `YOUR_PASSWORD`: Your SQL password

### Production (Azure)

In production, the connection string is retrieved from **Azure Key Vault** under the key `CigarTrackerDb`. 

To configure:
1. Set the `KeyVaultName` in `appsettings.json` (e.g., `"kv-cigar-tracker"`)
2. Add the Azure SQL connection string to Azure Key Vault with the name `CigarTrackerDb`

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
- `Brand` (nvarchar(255), Required)
- `Size` (nvarchar(100), Required)
- `Rating` (int, Required)
- `DateSmoked` (datetime2, Required)
- `Notes` (nvarchar(1000), Optional)

## Azure SQL Connection Notes

- **Host**: Azure SQL Database endpoint (e.g., `myserver.database.windows.net`)
- **Port**: 1433 (default MSSQL port)
- **Encryption**: Always enabled (Encrypt=True in connection string)
- **Authentication**: SQL Server authentication or Azure AD (Managed Identity in production)

## Troubleshooting

### Connection Errors

If you get connection errors:
1. Verify the Azure SQL server is running and accessible
2. Check the connection string in `appsettings.json` or `appsettings.Development.json`
3. Ensure the SQL login has permissions to create databases and tables
4. For Azure: Verify firewall rules allow your IP address
5. Verify the database exists on the Azure SQL server

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
