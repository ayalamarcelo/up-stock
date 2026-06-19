<h1 align="center">UpStock - Backend API</h1>

<p align="center">
  Backend del sistema <strong>UpStock</strong>, una plataforma para la gestión de activos y alquileres.
  Desarrollado en <strong>.NET 9</strong> con arquitectura limpia basada en Controladores, Servicios e Interfaces,
  integrado con <strong>PostgreSQL</strong> mediante Entity Framework Core.
</p>

---

## Tecnologías y Herramientas

| Tecnología | Uso |
|---|---|
| .NET 9 (Web API) | Framework principal del backend |
| PostgreSQL | Motor de base de datos relacional |
| Entity Framework Core (Npgsql) | ORM para mapeo de objetos a tablas |
| Serilog | Registro y auditoría de operaciones |
| JWT (JSON Web Tokens) | Autenticación stateless de usuarios |
| Swashbuckle / Swagger | Documentación interactiva de la API |
| DotNetEnv | Carga de variables de entorno desde `.env` |

---

## Arquitectura del Proyecto

El proyecto sigue el patrón **Controller → Interface → Service**, desacoplando la lógica de negocio de los endpoints HTTP:

```
backend/
├── Controllers/        → Endpoints de la API (HTTP GET, POST, PUT, DELETE)
├── DTOs/               → Objetos de transferencia de datos (Request/Response)
├── Interfaces/         → Contratos de los servicios (IAssetService, IAuthService, etc.)
├── Middlewares/        → Middlewares personalizados (Logging con Serilog)
├── Migrations/         → Historial de cambios de la base de datos (Entity Framework)
├── Models/             → Entidades C# que mapean a las tablas de PostgreSQL
├── Services/           → Lógica de negocio (implementación de las interfaces)
├── Program.cs          → Configuración central: DI, JWT, Swagger, middlewares
└── backend.csproj      → Paquetes NuGet del proyecto
```

---

## Guía de Configuración Inicial (Paso a Paso)

### 1. Prerrequisitos

Asegurate de tener instalado en tu máquina:
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL](https://www.postgresql.org/download/) (o una instancia en la nube)
- [pgAdmin](https://www.pgadmin.org/) (opcional, para visualizar la base de datos)

Verificá que el SDK esté instalado:
```bash
dotnet --list-sdks
```

---

### 2. Configurar las Variables de Entorno

El proyecto usa un archivo `.env` para guardar datos sensibles (contraseñas, claves JWT). Este archivo **nunca se sube a GitHub** por seguridad.

Copiá el archivo de ejemplo y renombralo:
```bash
# Dentro de la carpeta /backend
cp .env.example .env
```

Luego editá el archivo `.env` con tus datos reales:
```env
CONNECTION_STRING="Host=localhost;Port=5432;Database=upstock;Username=postgres;Password=tu_password"
JWT_KEY="UnaClaveSecretaMuyLargaYSeguraParaFirmarTokens"
JWT_ISSUER="UpStockAPI"
JWT_AUDIENCE="UpStockClient"
```

---

### 3. Configurar los Archivos de Configuración

De la misma forma, copiá los archivos de configuración de ejemplo:
```bash
cp appsettings.json.example appsettings.json
cp appsettings.Development.json.example appsettings.Development.json
```

Editá `appsettings.Development.json` para poner tu cadena de conexión local (la misma que en `.env`).

---

### 4. Crear la Base de Datos y Aplicar las Migraciones

Primero, creá una base de datos vacía llamada `upstock` en tu pgAdmin (o el nombre que hayas puesto en tu `.env`).

Luego, aplicá las migraciones para generar todas las tablas automáticamente:
```bash
dotnet ef database update
```

---

### 5. Compilar y Ejecutar el Proyecto

```bash
dotnet build
dotnet run
```

El servidor va a quedar escuchando en: **http://localhost:5102**

---

## Documentación de la API (Swagger)

Una vez que el servidor esté corriendo, accedé a la documentación interactiva en:

**http://localhost:5102/swagger**

### Cómo autenticarse en Swagger

1. Hacé un `POST` a `/api/Auth/register` para crear un usuario nuevo.
2. Copiá el valor del campo `token` de la respuesta.
3. Hacé click en el botón **Authorize** 🔒 (arriba a la derecha de Swagger).
4. Escribí `Bearer ` seguido de tu token y hacé click en **Authorize**.
5. A partir de ese momento, todas las peticiones van firmadas con tu usuario.

---

## Registro de Operaciones (Serilog)

El backend registra automáticamente todas las operaciones de escritura (`POST`, `PUT`, `DELETE`) en archivos de log diarios, incluyendo el **email real del usuario autenticado** que realizó cada acción.

Los archivos de log se generan en la carpeta:
```
backend/logs/upstock-YYYYMMDD.txt
```

Ejemplo de una línea de log:
```
2026-06-14 17:22:09 -03:00 [INF] (usuario@email.com) Operacion Iniciada: Usuario usuario@email.com ejecuto POST en /api/Asset
```