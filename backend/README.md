<h1 align="center">UpStock - Backend API</h1>

Este es el backend del sistema UpStock, desarrollado en .NET 10 utilizando una arquitectura limpia basada en Controladores y Servicios, e integrado con PostgreSQL mediante Entity Framework Core.

<p>TECNOLOGÍAS Y HERRAMIENTAS</p>

    Framework: .NET 10.0 (Web API)

    Base de Datos: PostgreSQL

    ORM: Entity Framework Core (versión 10.0)

    Documentación: OpenAPI / Swagger (Swashbuckle)

<p>ARQUITECTURA DEL PROYECTO</p>

El proyecto maneja una estructura desacoplada estándar para .NET. Las rutas no usan una carpeta independiente, sino que se resuelven directamente en los controladores mediante Enrutamiento por Atributos:

backend/
-- Controllers/      -> Endpoints de la API y manejo de rutas (HTTP GET, POST, etc.)
-- Models/           -> Modelos de datos (Entidades de C#) y el DbContext de Entity Framework
-- Services/         -> Lógica de negocio (Validaciones, cálculos y operaciones principales)
-- Program.cs        -> Configuración central de la app, inyección de dependencias y middlewares
-- backend.csproj    -> Archivo de configuración del proyecto y paquetes de NuGet

<p>GUÍA DE CONFIGURACIÓN INICIAL (PASO A PASO)</p>

Sigue estos pasos en orden para clonar, configurar y levantar el entorno de desarrollo en tu computadora local:

    Prerrequisitos
    Asegúrate de tener instalado el SDK de .NET 10 en tu máquina. Puedes verificarlo corriendo el siguiente comando en tu terminal:

dotnet --list-sdks

    Levantar la Base de Datos (PostgreSQL en Docker)
    Si no tienes una instancia local nativa de PostgreSQL, puedes levantar un contenedor rápido de Docker en el puerto 5432 ejecutando:

docker run --name sga-postgres -e POSTGRES_PASSWORD=tu_password -e POSTGRES_DB=tu_base_datos -p 5432:5432 -d postgres

(Nota: Asegúrate de crear las tablas necesarias en tu gestor de base de datos favorito, como pgAdmin o DBeaver, antes de pasar al siguiente paso).

    Instalación de Herramientas y Paquetes
    En caso de clonar el repositorio por primera vez, restaura los paquetes de NuGet necesarios ejecutando lo siguiente dentro de la carpeta backend:

dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Swashbuckle.AspNetCore

    Mapear la Base de Datos a C# (Scaffolding)
    Para leer las tablas de PostgreSQL y generar automáticamente los archivos dentro de la carpeta Models/, corre este comando (actualizando las credenciales por tus datos reales):

dotnet ef dbcontext scaffold "Host=localhost;Database=tu_base_datos;Username=postgres;Password=tu_password" Npgsql.EntityFrameworkCore.PostgreSQL -o Models --force

    Compilar y Ejecutar el Proyecto
    Finalmente, compila el código para asegurarte de que todo esté verde y levanta el servidor de desarrollo:

dotnet build
dotnet run

<p>DOCUMENTACIÓN DE LA API (OPENAPI / SWAGGER)</p>

El proyecto viene con Swagger integrado para que puedas probar los endpoints directamente desde una interfaz gráfica en el navegador de manera interactiva.

Una vez que el servidor esté corriendo con dotnet run, puedes acceder a la documentación mediante las siguientes URL:

Enlace HTTP: http://localhost:5000/swagger
Enlace HTTPS: https://localhost:5001/swagger