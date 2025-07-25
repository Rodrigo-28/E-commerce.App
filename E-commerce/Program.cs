using E_commerce.application.Extensions;
using E_commerce.application.Mappings;
using E_commerce.Domain.Common;
using E_commerce.Extensions;
using E_commerce.Helpers;
using E_commerce.Infrastructure.Contexts;
using E_commerce.Infrastructure.Extensions;
using E_commerce.Infrastructure.Seeding;
using E_commerce.Infrastructure.Services;
using E_commerce.Swagger;
using E_commerce.WebAPI.HostedServices;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        // La app seguirá serializando enums como strings en JSON
        opts.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

//Infrastructure
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();


// SignalR & in?memory queue & hosted service
builder.Services.AddSignalR();
builder.Services.AddSingleton<OrderQueue>();
// 1) Sigues registrando la instancia para inyección en tu controller
builder.Services.AddSingleton<OrderProcessingService>();

// 2) Y la misma instancia la usas como HostedService
//builder.Services.AddHostedService(sp => sp.GetRequiredService<OrderProcessingService>());


//Add Authorization and Authentication
//builder.Services.AddCustomAuth(builder.Configuration);

// para la coneccion del websocket
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddCustomSwagger();

// 1. Registrar Swagger en los servicios:
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Para Swashbuckle >= 6:
    c.UseInlineDefinitionsForEnums();

    // O para versiones anteriores:
    //c.DescribeAllEnumsAsStrings();
    c.ParameterFilter<EnumParameterFilter>();
});
//Configure DB Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);


//mapping
builder.Services.AddAutoMapper(typeof(UserProfile));

builder.Services.AddControllersWithViews();
builder.Services.AddCustomValidators();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();
app.UseAuthorization();
// 2. Mappea el endpoint del hub
app.MapHub<NotificationsHub>("/notifications");
app.UseDefaultFiles();  // Apunta a wwwroot/index.html por defecto
app.UseStaticFiles();   // Sirve /css, /js, /index.html, etc.

app.MapControllers();

app.Run();
