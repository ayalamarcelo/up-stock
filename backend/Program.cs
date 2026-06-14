using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UpStock.Data;
using UpStock.Services;
using UpStock.Interfaces;
using Serilog;
using UpStock.Middlewares;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Agregar servicios de Controladores
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IAuthService, AuthService>();

// 1. Configuración de Swagger para "Up Stock"
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Up Stock",
        Version = "v1",
        Description = "Backend para gestión de activos"
    });
});

// Registra el DbContext conectado a PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    .UseLowerCaseNamingConvention());

// =================================================================
// REGISTRO DE SERVICIOS DE LA APLICACIÓN (INYECCIÓN DE DEPENDENCIAS)
// =================================================================

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IStatusService, StatusService>();

// DESCOMENTAR A MEDIDA QUE SE CREAN LOS ARCHIVOS DE SERVICIOS:

// Para la gestión de Activos principales
builder.Services.AddScoped<IAssetService, AssetService>();

// Para el manejo de Usuarios y Autenticación
builder.Services.AddScoped<IUserService, UserService>();

// Para la administración de Clientes
builder.Services.AddScoped<IClientService, ClientService>();

// Para el núcleo del sistema: Alquileres y contratos
builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<IRentalItemService, RentalItemService>();

// Para los registros de reparaciones y auditoría (Opcionales si llevan servicios separados)
// builder.Services.AddScoped<IMaintenanceLogService, MaintenanceLogService>();
// builder.Services.AddScoped<IAssetLogService, AssetLogService>();

var app = builder.Build();

// 2. Configuración de la UI de Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Up Stock V1");
        c.DocumentTitle = "Up Stock API"; // Título en la pestaña del navegador
    });
}

app.UseHttpsRedirection();

app.UseMiddleware<UserLoggingMiddleware>();

app.MapControllers();

app.Run();