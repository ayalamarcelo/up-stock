using Microsoft.EntityFrameworkCore;
using UpStock.Data;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios de Controladores y EF Core
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registra el DbContext con la cadena de conexión
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Mapea controladores
app.MapControllers();

app.Run();