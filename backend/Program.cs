using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UpStock.Data;
using UpStock.Services;
using UpStock.Interfaces;
using Serilog;
using UpStock.Middlewares;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// CORS — permite que el frontend (file:// o localhost) llame al backend
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Agregar servicios de Controladores con configuración de nombres JSON en PascalCase
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IAuthService, AuthService>();

var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? "SuperSecretKeyForUpStockJWTAuth2026";
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "UpStockAPI";
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "UpStockClient";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// 1. Configuración de Swagger para "Up Stock" con soporte para JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Up Stock",
        Version = "v1",
        Description = "Backend para gestión de activos"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// Registra el DbContext conectado a PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    .UseLowerCaseNamingConvention()
    .ConfigureWarnings(warnings => warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));

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

// =====================================================
// SEED DATA — Datos iniciales si la BD está vacía
// =====================================================
try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (!db.Statuses.Any())
        {
            db.Statuses.AddRange(
                new UpStock.Models.Status { NameStatus = "Disponible" },
                new UpStock.Models.Status { NameStatus = "En uso" },
                new UpStock.Models.Status { NameStatus = "En mantenimiento" },
                new UpStock.Models.Status { NameStatus = "Fuera de servicio" }
            );
        }

        if (!db.Categories.Any())
        {
            db.Categories.AddRange(
                new UpStock.Models.Category { NameCategory = "Electrónica" },
                new UpStock.Models.Category { NameCategory = "Audio y Video" },
                new UpStock.Models.Category { NameCategory = "Iluminación" },
                new UpStock.Models.Category { NameCategory = "Muebles" },
                new UpStock.Models.Category { NameCategory = "Herramientas" }
            );
        }

        db.SaveChanges();
    }
}
catch (Exception ex)
{
    Log.Warning("No se pudo conectar a la base de datos para cargar los datos semilla: {Message}", ex.Message);
}

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

app.UseCors("FrontendPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<UserLoggingMiddleware>();

app.MapControllers();

app.Run();