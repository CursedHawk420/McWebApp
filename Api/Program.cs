using Highgeek.McWebApp.Common.Helpers;
using Highgeek.McWebApp.Common.Models;
using Highgeek.McWebApp.Common.Models.Contexts;
using Highgeek.McWebApp.Common.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Discord.Commands;
using Highgeek.McWebApp.Api.Services.Discord;
using Highgeek.McWebApp.Common.Services.Redis;
using Highgeek.McWebApp.Common.Options;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry;
using Prometheus;
using Highgeek.McWebApp.Common;



var builder = WebApplication.CreateBuilder(args);

//builder.AddServiceDefaults();
Environment.SetEnvironmentVariable("HIGHGEEK_APPNAME", "dotnet_api");

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
{
    Environment.SetEnvironmentVariable("HIGHGEEK_APPENV", "prod");
    builder.Configuration.SetBasePath("/appsettings/").AddJsonFile("appsettings.json").AddEnvironmentVariables();
}
else
{
    Environment.SetEnvironmentVariable("HIGHGEEK_APPENV", "dev");
    builder.Configuration.SetBasePath("/app/").AddJsonFile("appsettings.json").AddEnvironmentVariables();
}


builder.AddServiceDefaults();

var connectionStringUsers = ConfigProvider.GetConnectionString("PostgresUsersConnection");
var connectionStringKeys = ConfigProvider.GetConnectionString("PostgresKeysConnection");
var connectionStringCms = ConfigProvider.GetConnectionString("PostgresCmsConnection");

var connectionStringMC = ConfigProvider.GetConnectionString("MysqlMCServerConnection");
var connectionStringMC_data = ConfigProvider.GetConnectionString("MysqlMCServerConnection_mcserver_datadb");
var connectionStringMC_eco = ConfigProvider.GetConnectionString("MysqlMCServerConnection_mcserver_ecodb");
var connectionStringMC_husksync = ConfigProvider.GetConnectionString("MysqlMCServerConnection_mcserver_husksync");

//users dbcontext Postgress
builder.Services.AddDbContext<UsersDbContext>(options => options.UseNpgsql(connectionStringUsers), ServiceLifetime.Scoped);
// Add a DbContext to store your Database Keys
builder.Services.AddDbContext<KeysContext>(options => options.UseNpgsql(connectionStringKeys), ServiceLifetime.Scoped);
builder.Services.AddDbContext<McWebApp1CmsContext>(options => options.UseNpgsql(connectionStringCms), ServiceLifetime.Scoped);

builder.Services.AddDbContext<McserverMaindbContext>(options => options.UseMySql(connectionStringMC, MariaDbServerVersion.AutoDetect(connectionStringMC), providerOptions => providerOptions.EnableRetryOnFailure()), ServiceLifetime.Scoped);
builder.Services.AddDbContext<McserverDatadbContext>(options => options.UseMySql(connectionStringMC_data, MariaDbServerVersion.AutoDetect(connectionStringMC_data), providerOptions => providerOptions.EnableRetryOnFailure()), ServiceLifetime.Scoped);
builder.Services.AddDbContext<McserverEcoDataContext>(options => options.UseMySql(connectionStringMC_eco, MariaDbServerVersion.AutoDetect(connectionStringMC_eco), providerOptions => providerOptions.EnableRetryOnFailure()), ServiceLifetime.Scoped);
builder.Services.AddDbContext<McserverHusksyncContext>(options => options.UseMySql(connectionStringMC_husksync, MariaDbServerVersion.AutoDetect(connectionStringMC_husksync), providerOptions => providerOptions.EnableRetryOnFailure()), ServiceLifetime.Scoped);


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<UsersDbContext>();

builder.Services.AddSingleton<LuckPermsService>();
builder.Services.AddScoped<ImageCacheService>();
builder.Services.AddScoped<MinecraftUserManager>();

builder.Services.AddScoped<EcoParser>();
builder.Services.AddScoped<MinecraftUserManager>();
builder.Services.AddSingleton<MineskinApiCommunication>();
builder.Services.AddScoped<PteroManager>();
builder.Services.AddScoped<SkinManager>();

builder.Services.AddSingleton<IRedisUpdateService, RedisUpdateService>();

builder.Services.AddSingleton<RedisListenerService>();
builder.Services.AddHostedService(
    provider => provider.GetRequiredService<RedisListenerService>());

builder.Services.AddSingleton<CommandService>();
builder.Services.AddSingleton<DiscordBackgroundService>();
builder.Services.AddHostedService(
    provider => provider.GetRequiredService<DiscordBackgroundService>());

builder.Services.AddSingleton<IEmailSender, EmailSender>();

//MailKit options load
builder.Services.Configure<MailKitEmailSenderOptions>(options =>
{
    options.Host_Address = ConfigProvider.GetConfigString("ExternalProviders:MailKit:SMTP:Address");
    options.Host_Port = Convert.ToInt32(ConfigProvider.GetConfigString("ExternalProviders:MailKit:SMTP:Port"));
    options.Host_Username = ConfigProvider.GetConfigString("ExternalProviders:MailKit:SMTP:Account");
    options.Host_Password = ConfigProvider.GetConfigString("ExternalProviders:MailKit:SMTP:Password");
    options.Sender_EMail = ConfigProvider.GetConfigString("ExternalProviders:MailKit:SMTP:SenderEmail");
    options.Sender_Name = ConfigProvider.GetConfigString("ExternalProviders:MailKit:SMTP:SenderName");
});

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "HighGeek API",
        Description = "API For HighGeek services."
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

/*
builder.Services.UseHttpClientMetrics();
builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService("api"));
    options.IncludeFormattedMessage = true;
    options.IncludeScopes = true;

});
builder.Services.AddOpenTelemetry()
      .ConfigureResource(resource => resource.AddService("api"))
      .WithTracing(tracing => tracing
          .AddAspNetCoreInstrumentation())
      .WithMetrics(metrics => metrics
          .AddAspNetCoreInstrumentation()).UseOtlpExporter();*/

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseMetricServer();
app.UseHttpMetrics();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    
    app.UseSwaggerUI(options => 
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.MapGet("/random-number", () =>
{
    var number = Random.Shared.Next(0, 10);
    if (number >= 7)
        return Results.Unauthorized();
    return Results.Ok(number);
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
