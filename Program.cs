using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using cigar_tracker.Components;
using cigar_tracker.Data;
using cigar_tracker.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);



// Conditionally add Azure Key Vault as a configuration source in non-development environments
if (!builder.Environment.IsDevelopment())
{
	var keyVaultName = builder.Configuration["KeyVaultName"];
	if (!string.IsNullOrEmpty(keyVaultName))
	{
		var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");
		builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());
	}
}




// Add authentication services
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        options.Authority = "https://accounts.google.com";
        options.ClientId = builder.Configuration["GoogleAuthClientId"] ?? "YOUR_GOOGLE_CLIENT_ID";
        options.ClientSecret = builder.Configuration["GoogleAuthClientSecret"] ?? "YOUR_GOOGLE_CLIENT_SECRET";
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.TokenValidationParameters.NameClaimType = "name";
        options.SignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    });

// Add authorization
builder.Services.AddAuthorization();

// Add database context (Azure SQL)
var connectionString = builder.Configuration.GetConnectionString("CigarTrackerDb") 
    ?? throw new InvalidOperationException("Connection string 'CigarTrackerDb' not found.");
builder.Services.AddDbContext<CigarTrackerDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<CigarService>();
builder.Services.AddHttpClient<UpcService>();

var app = builder.Build();

// Apply any pending migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CigarTrackerDbContext>();
    db.Database.Migrate();
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

// Add authentication & authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add authentication endpoints for login/logout
app.MapPost("/login", async (HttpContext context) =>
{
    await context.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme);
    context.Response.Redirect("/");
});

app.MapPost("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
});

app.Run();
