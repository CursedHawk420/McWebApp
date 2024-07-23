using Highgeek.McWebApp.Common.Helpers;
using Highgeek.McWebApp.Common.Models;
using Highgeek.McWebApp.Common.Models.Contexts;
using Highgeek.McWebApp.Common.Services;
using Highgeek.McWebApp.Api.Services.Redis;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Discord.Commands;
using Highgeek.McWebApp.Api.Services.Discord;

var builder = WebApplication.CreateBuilder(args);

//builder.AddServiceDefaults();
var configuration = ConfigProvider.Instance;


var connectionStringUsers = configuration.GetConnectionString("PostgresUsersConnection");
var connectionStringKeys = configuration.GetConnectionString("PostgresKeysConnection");
var connectionStringCms = configuration.GetConnectionString("PostgresCmsConnection");

var connectionStringMC = configuration.GetConnectionString("MysqlMCServerConnection");
var connectionStringMC_data = configuration.GetConnectionString("MysqlMCServerConnection_mcserver_datadb");
var connectionStringMC_eco = configuration.GetConnectionString("MysqlMCServerConnection_mcserver_ecodb");
var connectionStringMC_husksync = configuration.GetConnectionString("MysqlMCServerConnection_mcserver_husksync");

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
builder.Services.AddScoped<MineskinApiCommunication>();
builder.Services.AddScoped<MineSkinApi.Client.Configuration>();
builder.Services.AddScoped<PteroManager>();
builder.Services.AddScoped<SkinManager>();

builder.Services.AddSingleton<IApiRedisUpdateService, ApiRedisUpdateService>();

builder.Services.AddSingleton<ApiRedisListenerService>();
builder.Services.AddHostedService(
    provider => provider.GetRequiredService<ApiRedisListenerService>());

builder.Services.AddSingleton<CommandService>();
builder.Services.AddSingleton<DiscordBackgroundService>();
builder.Services.AddHostedService(
    provider => provider.GetRequiredService<DiscordBackgroundService>());

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "HighGeek API",
        Description = "API For highgeek services."
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


var app = builder.Build();

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
