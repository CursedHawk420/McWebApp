using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using Highgeek.McWebApp.BlazorServer.Components;
using Highgeek.McWebApp.BlazorServer.Components.Account;

using Highgeek.McWebApp.Common;
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
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

//builder.AddServiceDefaults();


Environment.SetEnvironmentVariable("HIGHGEEK_APPNAME", "dotnet_blazorserver");

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
{
    Environment.SetEnvironmentVariable("HIGHGEEK_APPENV", "prod");

    builder.Configuration.SetBasePath("/appsettings/").AddJsonFile("appsettings.json").AddEnvironmentVariables();
}
else
{
    Environment.SetEnvironmentVariable("HIGHGEEK_APPENV", "dev");

    Environment.SetEnvironmentVariable("TENANT_ID", "dev");

    builder.Configuration.SetBasePath("/app/").AddJsonFile("appsettings.json").AddEnvironmentVariables();
}

builder.AddServiceDefaults();

var connectionStringUsers = ConfigProvider.GetConnectionString("PostgresUsersConnection");
var connectionStringKeys = ConfigProvider.GetConnectionString("PostgresKeysConnection");
var connectionStringCms = ConfigProvider.GetConnectionString("PostgresCmsConnection");

var connectionStringMC = ConfigProvider.GetConnectionString("MysqlMCServerConnection");
var connectionStringMC_data = ConfigProvider.GetConnectionString("MysqlMCServerConnection_mcserver_datadb");
var connectionStringMC_eco = ConfigProvider.GetConnectionString("MysqlMCServerConnection_mcserver_ecodb");
var connectionStringMC_plan = ConfigProvider.GetConnectionString("MysqlMCServerConnection_mcserver_plandb");

//users dbcontext Postgress
builder.Services.AddDbContext<UsersDbContext>(options => options.UseNpgsql(connectionStringUsers), ServiceLifetime.Scoped);
// Add a DbContext to store your Database Keys
builder.Services.AddDbContext<KeysContext>(options => options.UseNpgsql(connectionStringKeys), ServiceLifetime.Scoped);
builder.Services.AddDbContext<McWebApp1CmsContext>(options => options.UseNpgsql(connectionStringCms), ServiceLifetime.Scoped);

builder.Services.AddDbContext<McserverMaindbContext>(options => options.UseMySql(connectionStringMC, MariaDbServerVersion.AutoDetect(connectionStringMC), providerOptions => providerOptions.EnableRetryOnFailure()), ServiceLifetime.Scoped);
builder.Services.AddDbContext<McserverDatadbContext>(options => options.UseMySql(connectionStringMC_data, MariaDbServerVersion.AutoDetect(connectionStringMC_data), providerOptions => providerOptions.EnableRetryOnFailure()), ServiceLifetime.Scoped);
builder.Services.AddDbContext<McserverEcoDataContext>(options => options.UseMySql(connectionStringMC_eco, MariaDbServerVersion.AutoDetect(connectionStringMC_eco), providerOptions => providerOptions.EnableRetryOnFailure()), ServiceLifetime.Scoped);
builder.Services.AddDbContext<McserverPlandbContext>(options => options.UseMySql(connectionStringMC_plan, MariaDbServerVersion.AutoDetect(connectionStringMC_plan), providerOptions => providerOptions.EnableRetryOnFailure()), ServiceLifetime.Scoped);

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
builder.Services.AddScoped<IdentityUserAccessorStaticRender>();
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


builder.Services.AddIdentityCore<ApplicationUser>(options =>{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 10;
}).AddEntityFrameworkStores<UsersDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<TimeZoneService>();
builder.Services.AddScoped<ICookieService, CookieService>();

builder.Services.AddSingleton<ILanguageProvider, LanguageProvider>();
builder.Services.AddScoped<IPlanService, PlanService>();

builder.Services.AddScoped<ILocalizer, Localizer>();

builder.Services.AddScoped<EcoParser>();
builder.Services.AddScoped<MinecraftUserManager>();
builder.Services.AddSingleton<MineskinApiCommunication>();
builder.Services.AddScoped<PteroManager>();
builder.Services.AddScoped<SkinManager>();
builder.Services.AddScoped<IRefreshService, RefreshService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGameChatService, GameChatService>();

builder.Services.AddScoped<IInventoryService, InventoryService>();

builder.Services.AddSingleton<LuckPermsService>();
builder.Services.AddSingleton<IRedisUpdateService, RedisUpdateService>();
builder.Services.AddSingleton<IServerListService, ServerListService>();
builder.Services.AddScoped<ImageCacheService>();

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
        options.ExcludedMimeTypes = ["text/html"];
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

//user validation test
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(1);
});

builder.Services.AddSingleton<IConnectedUsersService, ConnectedUsersService>();


//MailKit options load
builder.Services.Configure<MailKitEmailSenderOptions>(options =>
{
    options.Host_Address = ConfigProvider.GetConfigString("ExternalProviders:MailKit:SMTP:Address");
    options.Host_Port = Convert.ToInt32(ConfigProvider.GetConfigString("ExternalProviders:MailKit:SMTP:Port"));
    options.Host_Username = ConfigProvider.GetConfigString("ExternalProviders:MailKit:SMTP:Account");
    options.Host_Password = ConfigProvider.GetConfigString("ExternalProviders:MailKit:SMTP:Password");
    options.Sender_EMail = ConfigProvider.GetConfigString("ExternalProviders:MailKit:SMTP:SenderEmail")  ;
    options.Sender_Name = ConfigProvider.GetConfigString("ExternalProviders:MailKit:SMTP:SenderName");
});
//Google auth register
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = ConfigProvider.GetConfigString("GoogleAuthOptions:ClientId");
    googleOptions.ClientSecret = ConfigProvider.GetConfigString("GoogleAuthOptions:ClientSecret");
    googleOptions.ReturnUrlParameter = "https://test.highgeek.eu/signin-google";
    googleOptions.CallbackPath = "/signin-google";
});
//Discord auth register
builder.Services.AddAuthentication().AddDiscord(discordOptions =>
{
    discordOptions.ClientId = ConfigProvider.GetConfigString("DiscordAuthOptions:ClientId");
    discordOptions.ClientSecret = ConfigProvider.GetConfigString("DiscordAuthOptions:ClientSecret");
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

builder.Services.UseHttpClientMetrics();



var app = builder.Build();

app.MapDefaultEndpoints();

app.UseMetricServer();
app.UseHttpMetrics();

//app.MapDefaultEndpoints();
if (app.Environment.IsProduction())
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
app.MapGet("/random-number", () =>
{
    var number = Random.Shared.Next(0, 10);
    if (number >= 7)
        return Results.Unauthorized();
    return Results.Ok(number);
});

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
