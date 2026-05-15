using Microsoft.OpenApi.Models; // Importa herramientas de Swagger-OpenAPI.

var builder = WebApplication.CreateBuilder(args); // Crea la aplicación backend

// Agrega servicios al proyecto (detecte endpoints.)
builder.Services.AddEndpointsApiExplorer();

// Configuración de Swagger
builder.Services.AddSwaggerGen(opciones =>
{
    opciones.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API UpStock",
        Version = "v1",
        Description = "Documentación de la API del sistema de gestión de activos"
    });
});

var app = builder.Build(); // Construye/inicia la app.

// Activa Swagger / Interfaz Visual
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // AS

    app.UseSwaggerUI(opciones => // IV
    {
        opciones.SwaggerEndpoint("/swagger/v1/swagger.json", "API UpStock v1");
    });
}

app.UseHttpsRedirection(); // Fuerza HTTPS seguro.

// Endpoint de ejemplo
app.MapGet("/users/{id}", (int id) =>
{
    return new
    {
        Id = id,
        Nombre = "Usuario de ejemplo"
    };
})
.WithName("GetUserById");


app.MapGet("/assets", () =>
{
    return new[]
    {
        new
        {
            Id = 1,
            Nombre = "Parlante JBL",
            Estado = "Disponible"
        }
    };
})
.WithName("GetAssets");


app.MapPost("/rentals", () =>
{
    return Results.Created("/rentals/1", new
    {
        Mensaje = "Alquiler creado correctamente"
    });
})
.WithName("CreateRental");

app.Run(); // Ejecuta el backend