<h1 align="center">Up Stock</h1>

<p align="center">
  <a href="https://learn.microsoft.com/es-es/dotnet/csharp/">
    <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white">
  </a>
  <a href="https://dotnet.microsoft.com/">
    <img src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white">
  </a>
  <a href="https://swagger.io/">
    <img src="https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black">
  </a>
  <a href="https://www.postgresql.org/">
    <img src="https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white">
  </a>
  <a href="https://learn.microsoft.com/es-es/ef/">
    <img src="https://img.shields.io/badge/Entity%20Framework-512BD4?style=for-the-badge&logo=dotnet&logoColor=white">
  </a>
  <a href="https://www.typescriptlang.org/">
    <img src="https://img.shields.io/badge/TypeScript-3178C6?style=for-the-badge&logo=typescript&logoColor=white">
  </a>
  <a href="https://angular.dev/">
    <img src="https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white">
  </a>
</p>

## Descripción

**Up Stock** es una plataforma integral diseñada para la administración logística y operativa de activos en empresas de servicios técnicos y eventos. El sistema permite controlar el ciclo de vida completo de cada activo garantizando disponibilidad, trazabilidad y mantenimiento preventivo.

## ¿Qué resuelve Up Stock?

La gestión de activos técnicos suele ser caótica por la alta rotación de equipos. Up Stock centraliza esta información para resolver tres pilares críticos:

* Control Total de Inventario: Permite identificar cada activo de forma única mediante códigos internos, categorizándolos y monitoreando su estado en tiempo real (Disponible, En uso, En mantenimiento, etc.).   
* Gestión de Logística y Rentas: Automatiza el flujo de despacho y retorno, vinculando activos específicos a contratos de alquiler y clientes determinados, asegurando que cada entrega y devolución esté documentada correctamente.   
* Trazabilidad y Mantenimiento: Registra un historial detallado de quién manipuló cada activo, cuándo salió, cuándo regresó y qué mantenimiento se le ha realizado. Esto permite mantener los equipos operativos y prever fallas antes de que lleguen al cliente. 

## Características Técnicas del Sistema:
* Gestión por Estados y Categorías: Estructura flexible para clasificar equipos y actualizar su condición operativa de forma centralizada.   
* Detalle de Entregas (RentalItems): Control preciso sobre los activos incluidos en cada contrato, permitiendo registrar la condición exacta de retorno de cada elemento.   
* Histórico de Movimientos (Logs): Auditoría completa de todas las acciones realizadas sobre los activos por parte de los usuarios del sistema.

## Documentación de la API

Este proyecto cuenta con una documentación interactiva generada con **Swagger/OpenAPI**.
Una vez que la aplicación esté ejecutándose, se puede acceder a la interfaz interactiva para probar los endpoints:

**[Ver Documentación de la API](http://localhost:5102/swagger)**

En esta interfaz se podrá ver:

* Los modelos de datos definidos con **Entity Framework**.
* Esquemas de solicitud y respuesta.
* Pruebas en tiempo real de cada controlador.


## Correr proyecto
Para poner en marcha el backend :

1. `cd backend`
2. `dotnet restore`
3. `dotnet run`
4. `dotnet ef database update --framework net9.0` 
5. Acceder a la API en: [http://localhost:5102/swagger](http://localhost:5102/swagger)

> [!NOTE]
> Solo si el paso 3 no funciona. Elegir la versión de .NET propia.