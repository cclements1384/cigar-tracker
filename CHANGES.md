# Cigar Tracker - User Tracking Update

## Changes Made (2026-02-24)

### 1. **Model Update** (`Models/Cigar.cs`)
- Added `LoggedInUser` property to track which authenticated user logged the cigar entry
- Property stores the email address of the logged-in user

### 2. **Database Context Update** (`Data/CigarTrackerDbContext.cs`)
- Configured `LoggedInUser` as a required field with max length 255 characters

### 3. **Migration** (`Migrations/20260224155659_AddLoggedInUserToCigar.cs`)
- Generated EF Core migration to add `LoggedInUser` column to the `Cigars` table
- Migration will auto-apply on app startup via `db.Database.Migrate()` in Program.cs

### 4. **UI Updates** (`Components/Pages/CigarLog.razor`)
- Updated `AddCigar()` method to capture current user email: `newCigar.LoggedInUser = userEmail`
- Added "Logged By" column to the cigar log table displaying which user added each entry
- Column shows user email in small text for easy identification

## Behavior

When a user logs in and adds a new cigar:
1. The app captures their email from the authentication provider (Google OAuth)
2. The email is automatically stored in the `LoggedInUser` field
3. The cigar log table now displays which user added each cigar entry
4. Multiple users can contribute to the same log, with each entry attributed to its creator

## Deployment

When you run the app:
1. The migration will automatically execute during startup
2. Existing cigar entries will have `LoggedInUser` set to an empty string (default value)
3. New entries will properly record the logged-in user
4. The database schema will be updated with the new column

## Notes

- The user email is captured from Google OAuth claims
- Falls back to "name" claim if email is not available
- All new entries will have the user properly recorded going forward
