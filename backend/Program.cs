using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models; // Necesario para OpenApiInfo
using UpStock.Data;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios de Controladores
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 1. Configuración de Swagger para "Up Stock"
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "API Up Stock", 
        Version = "v1",
        Description = "Backend para gestión de activos"
    });
});

// Registra el DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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
app.MapControllers();

app.Run();