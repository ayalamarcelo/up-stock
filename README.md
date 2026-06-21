<h1 align="center">Up Stock</h1>

<p align="center">
  <a href="https://learn.microsoft.com/es-es/dotnet/csharp/">
    <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white">
  </a>
  <a href="https://dotnet.microsoft.com/">
    <img src="https://img.shields.io/badge/.NET_9-512BD4?style=for-the-badge&logo=dotnet&logoColor=white">
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
  <a href="https://jwt.io/">
    <img src="https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white">
  </a>
  <a href="https://serilog.net/">
    <img src="https://img.shields.io/badge/Serilog-CC2222?style=for-the-badge&logo=serilog&logoColor=white">
  </a>
</p>

## Descripción

**Up Stock** es una plataforma integral diseñada para la administración logística y operativa de activos en empresas de servicios técnicos y eventos. El sistema permite controlar el ciclo de vida completo de cada activo garantizando disponibilidad, trazabilidad y mantenimiento preventivo.

## ¿Qué resuelve Up Stock?

La gestión de activos técnicos suele ser caótica por la alta rotación de equipos. Up Stock centraliza esta información para resolver tres pilares críticos:

* **Control Total de Inventario:** Permite identificar cada activo de forma única mediante códigos internos, categorizándolos y monitoreando su estado en tiempo real (Disponible, En uso, En mantenimiento, etc.).
* **Gestión de Logística y Rentas:** Automatiza el flujo de despacho y retorno, vinculando activos específicos a contratos de alquiler y clientes determinados, asegurando que cada entrega y devolución esté documentada correctamente.
* **Trazabilidad y Mantenimiento:** Registra un historial detallado de quién manipuló cada activo, cuándo salió, cuándo regresó y qué mantenimiento se le ha realizado. Esto permite mantener los equipos operativos y prever fallas antes de que lleguen al cliente.

## Características Técnicas del Sistema

* **Gestión por Estados y Categorías:** Estructura flexible para clasificar equipos y actualizar su condición operativa de forma centralizada.
* **Detalle de Entregas (RentalItems):** Control preciso sobre los activos incluidos en cada contrato, permitiendo registrar la condición exacta de retorno de cada elemento.
* **Autenticación JWT:** Sistema de login seguro con tokens firmados. Cada usuario autenticado recibe un token que autoriza sus peticiones a la API.
* **Auditoría con Serilog:** Registro automático de todas las operaciones de escritura (POST, PUT, DELETE) en archivos de log diarios, incluyendo el email del usuario que realizó cada acción.
* **Histórico de Movimientos (Logs):** Auditoría completa de todas las acciones realizadas sobre los activos por parte de los usuarios del sistema.

## Documentación de la API

Este proyecto cuenta con una documentación interactiva generada con **Swagger/OpenAPI**.
Una vez que la aplicación esté ejecutándose, se puede acceder a la interfaz interactiva para probar los endpoints:

**[Ver Documentación de la API](http://localhost:5102/swagger)**

En esta interfaz se podrá ver:

* Los modelos de datos definidos con **Entity Framework**.
* Esquemas de solicitud y respuesta.
* Pruebas en tiempo real de cada controlador.
* El botón **Authorize** 🔒 para autenticarse con un token JWT Bearer.

## Configuración del Entorno

Antes de correr el proyecto por primera vez, es necesario crear el archivo de variables de entorno `.env` dentro de la carpeta `backend/`:

```bash
# Copiar el archivo de ejemplo
cp backend/.env.example backend/.env
```

Luego editar el archivo `backend/.env` con los datos reales:

```env
CONNECTION_STRING="Host=localhost;Port=5432;Database=upstock;Username=postgres;Password=tu_password"
JWT_KEY="UnaClaveSecretaMuyLargaYSeguraParaFirmarTokens"
JWT_ISSUER="UpStockAPI"
JWT_AUDIENCE="UpStockClient"
```

También copiar los archivos de configuración:

```bash
cp backend/appsettings.json.example backend/appsettings.json
cp backend/appsettings.Development.json.example backend/appsettings.Development.json
```

## Correr proyecto

Desde raíz:

```bash
dotnet run --project backend
```

Para poner en marcha el backend:

1. `cd backend`
2. `dotnet restore`
3. `dotnet ef database update`
4. `dotnet run`
5. Acceder a la API en: [http://localhost:5102/swagger](http://localhost:5102/swagger)

> [!NOTE]
> El paso 3 (migraciones) crea todas las tablas automáticamente en tu base de datos PostgreSQL. Asegurate de tener la base de datos creada y el archivo `.env` configurado antes de correr este paso.

## Correr absolutamente todo

```bash
dotnet test
```

## Correr solo las pruebas BDD del backend

```bash
dotnet test backend.Tests/backend.Tests.csproj
```

## Logs del Sistema

Serilog genera archivos de log diarios en la carpeta `backend/logs/`. Cada operación de escritura queda registrada con el email del usuario autenticado:

```
2026-06-14 17:22:09 -03:00 [INF] (usuario@email.com) Operacion Iniciada: Usuario usuario@email.com ejecuto POST en /api/Asset
```

##  Pruebas Unitarias

Las pruebas unitarias se encuentran en el proyecto `backend.Tests` y cubren todos los controladores de la API.

### Herramientas utilizadas
- **xUnit** → Framework de pruebas
- **Moq** → Simulación de servicios (sin base de datos real)
- **FluentAssertions** → Verificaciones legibles

### Cobertura

| Controlador        | Pruebas |
| CategoryController | 5 |
| AssetController    | 10|
| StatusController   | 8 |
| ClientsController  | 7 |
| AuthController     | 5 |
| RentalsController  | 4 |
| **Total**        | **39** |

---

### Detalle de pruebas por controlador

#### CategoryController
| Caso de prueba | Estado esperado | Resultado |
|---|---|---|
| GetCategories_Retorna200_CuandoHayCategorias | 200 OK | 
| GetCategories_Retorna404_CuandoNoHayCategorias | 404 Not Found | 
| PostCategory_Retorna201_CuandoDatosValidos | 201 Created | 
| PostCategory_Retorna400_CuandoNombreEsVacio | 400 Bad Request | 
| DeleteCategory_Retorna404_CuandoNoExiste | 404 Not Found | 

#### AssetController
| Caso de prueba | Estado esperado | Resultado |
|---|---|---|
| GetAssets_Retorna200_CuandoHayActivos | 200 OK | 
| GetAssets_Retorna404_CuandoNoHayActivos | 404 Not Found | 
| GetAsset_Retorna200_CuandoExiste | 200 OK | 
| GetAsset_Retorna404_CuandoNoExiste | 404 Not Found | 
| PostAsset_Retorna201_CuandoDatosValidos | 201 Created | 
| PostAsset_Retorna400_CuandoNombreEsVacio | 400 Bad Request | 
| PostAsset_Retorna400_CuandoCodigoEsVacio | 400 Bad Request | 
| PostAsset_Retorna400_CuandoCategoriaEsVacia | 400 Bad Request | 
| PutAsset_Retorna400_CuandoIdsNoCoinciden | 400 Bad Request | 
| DeleteAsset_Retorna404_CuandoNoExiste | 404 Not Found | 

#### StatusController
| Caso de prueba | Estado esperado | Resultado |
|---|---|---|
| GetStatuses_Retorna200_CuandoHayEstados | 200 OK | 
| GetStatuses_Retorna404_CuandoNoHayEstados | 404 Not Found | 
| GetStatus_Retorna200_CuandoExiste | 200 OK | 
| GetStatus_Retorna404_CuandoNoExiste | 404 Not Found | 
| PostStatus_Retorna201_CuandoDatosValidos | 201 Created | 
| PostStatus_Retorna400_CuandoNombreEsVacio | 400 Bad Request | 
| PutStatus_Retorna400_CuandoIdsNoCoinciden | 400 Bad Request | 
| DeleteStatus_Retorna404_CuandoNoExiste | 404 Not Found | 

#### ClientsController
| Caso de prueba | Estado esperado | Resultado |
|---|---|---|
| GetClients_Retorna200_CuandoHayClientes | 200 OK | 
| GetClient_Retorna200_CuandoExiste | 200 OK | 
| GetClient_Retorna404_CuandoNoExiste | 404 Not Found | 
| CreateClient_Retorna201_CuandoDatosValidos | 201 Created | 
| CreateClient_Retorna400_CuandoNombreEsVacio | 400 Bad Request | 
| CreateClient_Retorna400_CuandoDniCuitEsVacio | 400 Bad Request | 
| DeleteClient_Retorna404_CuandoNoExiste | 404 Not Found | 

#### AuthController
| Caso de prueba | Estado esperado | Resultado |
|---|---|---|
| Register_Retorna200_CuandoDatosValidos | 200 OK | 
| Register_Retorna400_CuandoEmailYaExiste | 400 Bad Request | 
| Login_Retorna200_CuandoCredencialesValidas | 200 OK | 
| Login_Retorna401_CuandoCredencialesInvalidas | 401 Unauthorized | 
| Login_Retorna401_CuandoUsuarioNoExiste | 401 Unauthorized | 

#### RentalsController
| Caso de prueba | Estado esperado | Resultado |
|---|---|---|
| GetRentals_Retorna200_CuandoHayAlquileres | 200 OK | 
| GetRental_Retorna200_CuandoExiste | 200 OK | 
| GetRental_Retorna404_CuandoNoExiste | 404 Not Found | 
| PostRental_Retorna201_CuandoDatosValidos | 201 Created | 

### Cómo ejecutar las pruebas

```powershell
cd C:\ruta\del\proyecto
dotnet test
```

### Resultado esperado
```
Resumen de pruebas: total: 39; con errores: 0; correcto: 39
```