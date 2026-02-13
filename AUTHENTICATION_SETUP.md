# Google OIDC Authentication Setup

This guide explains how to set up Google OIDC authentication for the Cigar Tracker application.

## Prerequisites

- Google Cloud Console account
- Cigar Tracker application running locally or deployed

## Step 1: Create a Google Cloud Project

1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Click on the project dropdown and select **"New Project"**
3. Enter a name (e.g., "Cigar Tracker") and click **Create**
4. Wait for the project to be created

## Step 2: Enable the Google+ API

1. In the Google Cloud Console, search for **"Google+ API"** in the search bar
2. Click on **Google+ API** from the results
3. Click **Enable**

## Step 3: Create OAuth 2.0 Credentials

1. In the left sidebar, go to **Credentials**
2. Click **+ Create Credentials** → **OAuth client ID**
3. If prompted, configure the OAuth consent screen first:
   - Select **External** for User Type
   - Fill in the required fields (App name, User support email, Developer contact)
   - In the "Scopes" section, add: `openid`, `email`, `profile`
   - Complete the consent screen

4. Back to OAuth client ID creation:
   - Select **Web application** as the application type
   - Give it a name (e.g., "Cigar Tracker Web")
   - Under **Authorized redirect URIs**, add:
     - `https://localhost:7123/signin-oidc` (local development)
     - `https://yourdomain.com/signin-oidc` (for production)
   - Click **Create**

5. Copy your credentials:
   - **Client ID** (looks like: `xxxxx.apps.googleusercontent.com`)
   - **Client Secret**

## Step 4: Configure the Application

### Local Development

1. Open `appsettings.Development.json` (or create it if it doesn't exist):

```json
{
  "GoogleAuth": {
    "ClientId": "YOUR_CLIENT_ID.apps.googleusercontent.com",
    "ClientSecret": "YOUR_CLIENT_SECRET"
  }
}
```

2. Replace `YOUR_CLIENT_ID` and `YOUR_CLIENT_SECRET` with your credentials from Google Cloud Console

### Production

1. Update `appsettings.json` OR use environment variables (recommended for security):

```bash
export GoogleAuth__ClientId=your_client_id
export GoogleAuth__ClientSecret=your_client_secret
```

## Step 5: Test the Application

1. Run the application:
   ```bash
   dotnet run
   ```

2. Navigate to `https://localhost:7123` (or your configured port)

3. You should see a "Login with Google" button in the top right

4. Click the button to authenticate with your Google account

5. After successful authentication, you'll be redirected to the Cigar Log page

6. You should see your email address displayed in the header with a "Logout" button

## Troubleshooting

### Redirect URI Mismatch Error
- Ensure the redirect URIs in Google Cloud Console exactly match your application's URLs
- For local dev, use `https://localhost:7123/signin-oidc` (note the HTTPS)

### Claims Not Displaying
- Verify that your Google OAuth consent screen includes the `email` and `profile` scopes
- Check that `GetClaimsFromUserInfoEndpoint = true` is set in Program.cs

### Port Number Issues
- If your app runs on a different port, update both:
  - The redirect URI in Google Cloud Console
  - The `appsettings.json` if needed

## Security Best Practices

1. **Never commit secrets**: Keep `ClientSecret` out of version control
2. **Use environment variables** for production
3. **Enable HTTPS only** in production
4. **Regularly rotate credentials** in Google Cloud Console
5. **Restrict redirect URIs** to only your application domains

## Additional Resources

- [Google Identity Documentation](https://developers.google.com/identity/protocols/oauth2)
- [ASP.NET Core OIDC Documentation](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins)
- [Microsoft Identity Platform](https://learn.microsoft.com/en-us/azure/active-directory/develop/)
