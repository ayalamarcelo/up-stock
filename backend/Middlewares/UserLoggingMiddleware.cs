using Serilog.Context;

namespace UpStock.Middlewares;

public class UserLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UserLoggingMiddleware> _logger;

    public UserLoggingMiddleware(RequestDelegate next, ILogger<UserLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 1. Intentar obtener el usuario (de JWT claims o de una cabecera temporal)
        string userEmail = "Anonimo";
        
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var emailClaim = context.User.FindFirst(System.Security.Claims.ClaimTypes.Email);
            userEmail = emailClaim?.Value ?? context.User.Identity.Name ?? "Autenticado";
        }
        else if (context.Request.Headers.TryGetValue("X-User-Email", out var emailHeader))
        {
            userEmail = emailHeader.ToString();
        }

        // 2. Empujar el email al contexto de Serilog para que aparezca en todos los logs de esta peticion
        using (LogContext.PushProperty("UserEmail", userEmail))
        {
            var method = context.Request.Method;
            var path = context.Request.Path;

            // Loguear operaciones de escritura/modificación (POST, PUT, DELETE)
            if (method == "POST" || method == "PUT" || method == "DELETE")
            {
                _logger.LogInformation("Operacion Iniciada: Usuario {UserEmail} ejecuto {Method} en {Path}", userEmail, method, path);
            }

            await _next(context);
        }
    }
}
