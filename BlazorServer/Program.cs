using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using Highgeek.McWebApp.BlazorServer.Components;
using Highgeek.McWebApp.BlazorServer.Components.Account;

using Highgeek.McWebApp.Common.Permissions;
using Highgeek.McWebApp.Common.Models;
using Highgeek.McWebApp.Common.Models.Contexts;
using Highgeek.McWebApp.Common.Options;
using Highgeek.McWebApp.Common.Services;
using Highgeek.McWebApp.Common.Permissions.Requirements;
using Highgeek.McWebApp.Common.Helpers;
using Highgeek.McWebApp.Common.Services.Redis;

using MudBlazor.Services;
using MudBlazor;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;


var builder = WebApplication.CreateBuilder(args);

//builder.AddServiceDefaults();
var configuration = ConfigProvider.Instance;


var connectionStringUsers = configuration.GetConnectionString("PostgresUsersConnection");
var connectionStringKeys = configuration.GetConnectionString("PostgresKeysConnection");
var connectionStringCms = configuration.GetConnectionString("PostgresCmsConnection");

var connectionStringMC = configuration.GetConnectionString("MysqlMCServerConnection");
var connectionStringMC_data = configuration.GetConnectionString("MysqlMCServerConnection_mcserver_datadb");
var connectionStringMC_eco = configuration.GetConnectionString("MysqlMCServerConnection_mcserver_ecodb");

//users dbcontext Postgress
builder.Services.AddDbContext<UsersDbContext>(options => options.UseNpgsql(connectionStringUsers), ServiceLifetime.Scoped);
// Add a DbContext to store your Database Keys
builder.Services.AddDbContext<KeysContext>(options => options.UseNpgsql(connectionStringKeys), ServiceLifetime.Scoped);
builder.Services.AddDbContext<McWebApp1CmsContext>(options => options.UseNpgsql(connectionStringCms), ServiceLifetime.Scoped);

builder.Services.AddDbContext<McserverMaindbContext>(options => options.UseMySql(connectionStringMC, MariaDbServerVersion.AutoDetect(connectionStringMC), providerOptions => providerOptions.EnableRetryOnFailure()), ServiceLifetime.Scoped);
builder.Services.AddDbContext<McserverDatadbContext>(options => options.UseMySql(connectionStringMC_data, MariaDbServerVersion.AutoDetect(connectionStringMC_data), providerOptions => providerOptions.EnableRetryOnFailure()), ServiceLifetime.Scoped);
builder.Services.AddDbContext<McserverEcoDataContext>(options => options.UseMySql(connectionStringMC_eco, MariaDbServerVersion.AutoDetect(connectionStringMC_eco), providerOptions => providerOptions.EnableRetryOnFailure()), ServiceLifetime.Scoped);

builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddServerSideBlazor(options =>
{
    options.DetailedErrors = builder.Environment.IsDevelopment();
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("group.sa", policy => policy.Requirements.Add(new PermissionsAuthorizeAttribute("group.sa")));
    options.AddPolicy("group.default", policy => policy.Requirements.Add(new PermissionsAuthorizeAttribute("group.default")));
    options.AddPolicy("connectedaccount", policy => policy.Requirements.Add(new PermissionsAuthorizeAttribute("connectedaccount")));
});

builder.Services.AddScoped<IAuthorizationHandler, PermissionsAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionsPolicyProvider>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddDataProtection().PersistKeysToDbContext<KeysContext>().SetApplicationName("mcWebApp");

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();
builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<UsersDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<TimeZoneService>();

builder.Services.AddScoped<EcoParser>();
builder.Services.AddScoped<MinecraftUserManager>();
builder.Services.AddScoped<MineskinApiCommunication>();
builder.Services.AddScoped<MineSkinApi.Client.Configuration>();
builder.Services.AddScoped<PteroManager>();
builder.Services.AddScoped<SkinManager>();
builder.Services.AddScoped<IRefreshService, RefreshService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<GameChatService>();

builder.Services.AddScoped<InventoryManagerService>();
builder.Services.AddScoped<ImageCacheService>();

builder.Services.AddSingleton<LuckPermsService>();
builder.Services.AddSingleton<IRedisUpdateService, RedisUpdateService>();

builder.Services.AddSingleton<RedisListenerService>();
builder.Services.AddHostedService(
    provider => provider.GetRequiredService<RedisListenerService>());

builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddSingleton<IdentityNoOpEmailSender>();

if (builder.Environment.IsProduction())
{

    builder.Services.AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
        options.Providers.Add<BrotliCompressionProvider>();
        options.Providers.Add<GzipCompressionProvider>();
    });
    builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
    {
        options.Level = CompressionLevel.Optimal;
    });

    builder.Services.Configure<GzipCompressionProviderOptions>(options =>
    {
        options.Level = CompressionLevel.Optimal;
    });
}


//MailKit options load
builder.Services.Configure<MailKitEmailSenderOptions>(options =>
{
    options.Host_Address = configuration.GetConfigString("ExternalProviders:MailKit:SMTP:Address");
    options.Host_Port = Convert.ToInt32(configuration.GetConfigString("ExternalProviders:MailKit:SMTP:Port"));
    options.Host_Username = configuration.GetConfigString("ExternalProviders:MailKit:SMTP:Account");
    options.Host_Password = configuration.GetConfigString("ExternalProviders:MailKit:SMTP:Password");
    options.Sender_EMail = configuration.GetConfigString("ExternalProviders:MailKit:SMTP:SenderEmail")  ;
    options.Sender_Name = configuration.GetConfigString("ExternalProviders:MailKit:SMTP:SenderName");
});
//Google auth register
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration.GetConfigString("GoogleAuthOptions:ClientId");
    googleOptions.ClientSecret = configuration.GetConfigString("GoogleAuthOptions:ClientSecret");
    googleOptions.ReturnUrlParameter = "https://test.highgeek.eu/signin-google";
    googleOptions.CallbackPath = "/signin-google";
});
//Discord auth register
builder.Services.AddAuthentication().AddDiscord(discordOptions =>
{
    discordOptions.ClientId = configuration.GetConfigString("DiscordAuthOptions:ClientId");
    discordOptions.ClientSecret = configuration.GetConfigString("DiscordAuthOptions:ClientSecret");
    discordOptions.ReturnUrlParameter = "https://test.highgeek.eu/signin-discord";
    discordOptions.CallbackPath = "/signin-discord";
    discordOptions.Scope.Add("email");
});
//Add MudBlazor
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 5000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});


var app = builder.Build();

//app.MapDefaultEndpoints();
if (!app.Environment.IsDevelopment())
{
    app.UseResponseCompression();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
