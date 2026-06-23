# Testing Unitario — Up Stock API
 
## Proyecto
UpStock - Sistema de Gestión de Alquiler y Control de Activos
 
## Objetivo
Validar de forma automatizada el comportamiento de los controladores de la API, asegurando que respondan correctamente ante distintos escenarios sin necesidad de levantar el servidor ni la base de datos real.
 
Las pruebas unitarias permiten detectar regresiones rápidamente cada vez que se modifica el código, manteniendo la calidad del sistema a lo largo del tiempo.
 
---
 
## Entorno de pruebas
 
### Herramientas utilizadas
- **xUnit** → Framework de pruebas para .NET
- **Moq** → Simulación de servicios y dependencias
- **FluentAssertions** → Verificaciones legibles y expresivas
- **Visual Studio / VS Code** → Entorno de desarrollo
### Datos del entorno
| Campo | Valor |
|---|---|
| Proyecto de tests | `backend.Tests` |
| Framework | .NET 9.0 |
| Total de pruebas | 39 |
| Tiempo de ejecución | ~3 segundos |
| Fecha de ejecución | 21/06/2026 |
 
### Cómo ejecutar las pruebas
```powershell
cd backend.Tests
dotnet test
```
 
### Resultado esperado
```
Resumen de pruebas: total: 39; con errores: 0; correcto: 39
```
 
---
 
## Patrón utilizado — AAA (Arrange-Act-Assert)
 
Todas las pruebas siguen la estructura estándar de testing unitario:
 
```csharp
[Fact]
public async Task GetCategories_Retorna200_CuandoHayCategorias()
{
    // Arrange: preparar los datos falsos
    var categorias = new List<Category>
    {
        new() { CategoryID = Guid.NewGuid(), NameCategory = "Audio" },
        new() { CategoryID = Guid.NewGuid(), NameCategory = "Iluminación" }
    };
    _mockService.Setup(s => s.GetAllAsync(1, 10)).ReturnsAsync(categorias);
 
    // Act: ejecutar el endpoint
    var result = await _controller.GetCategories();
 
    // Assert: verificar el resultado
    result.Result.Should().BeOfType<OkObjectResult>();
}
```
 
---
 
## Resumen por controlador
 
| Controlador | Cantidad | Cobertura |
|---|---|---|
| CategoryController | 5 | GET, POST, DELETE |
| AssetController | 10 | CRUD completo |
| StatusController | 8 | CRUD completo |
| ClientsController | 7 | CRUD completo |
| AuthController | 5 | Register, Login |
| RentalsController | 4 | GET, POST |
| **Total** | **39** | — |
 
---
 
## Detalle de pruebas por controlador
 
### CategoryController (5 pruebas)
| # | Prueba | Resultado esperado |
|---|---|---|
| 1 | GetCategories_Retorna200_CuandoHayCategorias | 200 OK |
| 2 | GetCategories_Retorna404_CuandoNoHayCategorias | 404 Not Found |
| 3 | PostCategory_Retorna201_CuandoDatosValidos | 201 Created |
| 4 | PostCategory_Retorna400_CuandoNombreEsVacio | 400 Bad Request |
| 5 | DeleteCategory_Retorna404_CuandoNoExiste | 404 Not Found |
 
### AssetController (10 pruebas)
| # | Prueba | Resultado esperado |
|---|---|---|
| 1 | GetAssets_Retorna200_CuandoHayActivos | 200 OK |
| 2 | GetAssets_Retorna404_CuandoNoHayActivos | 404 Not Found |
| 3 | GetAsset_Retorna200_CuandoExiste | 200 OK |
| 4 | GetAsset_Retorna404_CuandoNoExiste | 404 Not Found |
| 5 | PostAsset_Retorna201_CuandoDatosValidos | 201 Created |
| 6 | PostAsset_Retorna400_CuandoNombreEsVacio | 400 Bad Request |
| 7 | PostAsset_Retorna400_CuandoCodigoEsVacio | 400 Bad Request |
| 8 | PostAsset_Retorna400_CuandoCategoriaEsVacia | 400 Bad Request |
| 9 | PutAsset_Retorna400_CuandoIdsNoCoinciden | 400 Bad Request |
| 10 | DeleteAsset_Retorna404_CuandoNoExiste | 404 Not Found |
 
### StatusController (8 pruebas)
| # | Prueba | Resultado esperado |
|---|---|---|
| 1 | GetStatuses_Retorna200_CuandoHayEstados | 200 OK |
| 2 | GetStatuses_Retorna404_CuandoNoHayEstados | 404 Not Found |
| 3 | GetStatus_Retorna200_CuandoExiste | 200 OK |
| 4 | GetStatus_Retorna404_CuandoNoExiste | 404 Not Found |
| 5 | PostStatus_Retorna201_CuandoDatosValidos | 201 Created |
| 6 | PostStatus_Retorna400_CuandoNombreEsVacio | 400 Bad Request |
| 7 | PutStatus_Retorna400_CuandoIdsNoCoinciden | 400 Bad Request |
| 8 | DeleteStatus_Retorna404_CuandoNoExiste | 404 Not Found |
 
### ClientsController (7 pruebas)
| # | Prueba | Resultado esperado |
|---|---|---|
| 1 | GetClients_Retorna200_CuandoHayClientes | 200 OK |
| 2 | GetClient_Retorna200_CuandoExiste | 200 OK |
| 3 | GetClient_Retorna404_CuandoNoExiste | 404 Not Found |
| 4 | CreateClient_Retorna201_CuandoDatosValidos | 201 Created |
| 5 | CreateClient_Retorna400_CuandoNombreEsVacio | 400 Bad Request |
| 6 | CreateClient_Retorna400_CuandoDniCuitEsVacio | 400 Bad Request |
| 7 | DeleteClient_Retorna404_CuandoNoExiste | 404 Not Found |
 
### AuthController (5 pruebas)
| # | Prueba | Resultado esperado |
|---|---|---|
| 1 | Register_Retorna200_CuandoDatosValidos | 200 OK |
| 2 | Register_Retorna400_CuandoEmailYaExiste | 400 Bad Request |
| 3 | Login_Retorna200_CuandoCredencialesValidas | 200 OK |
| 4 | Login_Retorna401_CuandoCredencialesInvalidas | 401 Unauthorized |
| 5 | Login_Retorna401_CuandoUsuarioNoExiste | 401 Unauthorized |
 
### RentalsController (4 pruebas)
| # | Prueba | Resultado esperado |
|---|---|---|
| 1 | GetRentals_Retorna200_CuandoHayAlquileres | 200 OK |
| 2 | GetRental_Retorna200_CuandoExiste | 200 OK |
| 3 | GetRental_Retorna404_CuandoNoExiste | 404 Not Found |
| 4 | PostRental_Retorna201_CuandoDatosValidos | 201 Created |
 
---
 
## Resumen por código HTTP
 
| Tipo | Descripción | Cantidad |
|---|---|---|
| 200 OK | Endpoints respondiendo correctamente con datos válidos | 10 |
| 201 Created | Recursos creados exitosamente | 6 |
| 400 Bad Request | Validaciones de campos obligatorios o IDs inválidos | 13 |
| 401 Unauthorized | Credenciales incorrectas o usuario inexistente | 2 |
| 404 Not Found | Listados vacíos o IDs inexistentes | 8 |
| **Total** | | **39** |
 
---
 
## ¿Por qué usar Mock en lugar de la base de datos real?
 
Las pruebas unitarias usan **Moq** para simular los servicios y evitar depender de la base de datos. Esto trae varios beneficios:
 
| Aspecto | Con Mock | Con BD real |
|---|---|---|
| Velocidad | Milisegundos | Segundos por prueba |
| Necesita PostgreSQL corriendo | No | Sí |
| Pruebas independientes entre sí | Sí | No (se pisan los datos) |
| Simular errores | Trivial | Difícil |
| Resultado | Siempre el mismo | Depende del estado de la BD |
 
---
 
## Comparativa con pruebas manuales
 
| Aspecto | Pruebas Manuales (Gherkin) | Pruebas Unitarias |
|---|---|---|
| Quién ejecuta | Persona en Swagger | Computadora |
| Tiempo total | ~1-2 horas | ~3 segundos |
| Necesita BD | Sí | No |
| Necesita servidor | Sí | No |
| Evidencia | Capturas de pantalla | Resultado de `dotnet test` |
| Cuándo se usan | Verificación final, demos | Durante desarrollo y CI/CD |
| Total de casos | 58 | 39 |
 
---
 
## Conclusiones
 
- Las 39 pruebas unitarias se ejecutan correctamente en menos de 3 segundos
- Cubren los principales escenarios de éxito y error de cada controlador
- No requieren configuración previa (PostgreSQL, datos semilla, autenticación)
- Permiten detectar regresiones de forma inmediata al modificar el código
- Complementan las pruebas manuales en Swagger documentadas en `testing-manual.md`