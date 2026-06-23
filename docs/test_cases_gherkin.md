# Testing Manual — Up Stock API

## Proyecto
UpStock - Sistema de Gestión de Alquiler y Control de Activos

## Objetivo
El objetivo de estas pruebas fue verificar el correcto funcionamiento de los 
endpoints expuestos por la API, validando operaciones de consulta, creación, 
modificación y eliminación de registros, así como el manejo de errores y 
validaciones en cada módulo del sistema.

Las pruebas fueron diseñadas siguiendo el formato **Gherkin** 
(Given/When/Then) para documentar de forma clara las precondiciones, 
acciones y resultados esperados de cada caso.

---

## Entorno de pruebas

### Herramientas utilizadas
- **Swagger UI** → ejecución y validación de endpoints
- **ASP.NET Core Web API** → framework del backend
- **PostgreSQL 18** → base de datos
- **pgAdmin 4** → administración de la base de datos
- **Visual Studio / VS Code** → entorno de desarrollo

### Datos del entorno
| Campo | Valor |
|---|---|
| URL base | `http://localhost:5102` |
| Documentación | `http://localhost:5102/swagger` |
| Base de datos | UpStockDb |
| Framework | .NET 9.0 |
| Fecha de ejecución | 21/06/2026 |

### Configuración previa realizada
Durante la configuración inicial se detectó el siguiente error:

> `"The ConnectionString property has not been initialized."`

Para resolverlo se realizaron los siguientes pasos:

1. Instalar PostgreSQL 18
2. Crear la base de datos `UpStockDb` en pgAdmin
3. Crear el archivo `appsettings.json` con la cadena de conexión correcta
4. Ejecutar las migraciones con `dotnet ef database update`
5. Ejecutar la aplicación con `dotnet run`

Una vez completada la configuración, la API se conectó correctamente a la 
base de datos y los datos semilla se cargaron automáticamente:
- **4 estados:** Disponible, En uso, En mantenimiento, Fuera de servicio
- **5 categorías:** Electrónica, Audio y Video, Iluminación, Muebles, Herramientas

### Autenticación
Los endpoints protegidos requieren token JWT. Para obtenerlo:
1. Ejecutar **POST /api/auth/register** para registrar un usuario
2. Ejecutar **POST /api/auth/login** para obtener el token
3. En Swagger click en **Authorize 🔒** e ingresar `Bearer {token}`

### Cómo provocar errores 500
Para probar los casos de error interno del servidor:
- Modificar la contraseña en `appsettings.json` por una incorrecta mientras 
el servidor está corriendo
- Restaurar la contraseña correcta una vez tomada la evidencia

### Criterios de severidad
- **🔴 Alta:** Funcionalidad principal del CRUD (crear, listar, actualizar, eliminar registros válidos)
- **🟡 Media:** Validaciones de entrada, errores 404 y casos de borde
- **🟢 Baja:** Errores 500 (requieren simular caída del entorno)

---

## MÓDULO 1 — AUTENTICACIÓN
 
### CP-AUTH-01 — Registro exitoso
```gherkin
Feature: Autenticación de usuarios
 
  Scenario: Registro exitoso de un nuevo usuario
    Given que el endpoint POST /api/auth/register está disponible
    And el email "test@mail.com" no está registrado en el sistema
    When envío un POST a /api/auth/register con el body:
      | email    | test@mail.com |
      | password | 123456        |
    Then la respuesta debe tener el código 200 OK
    And la respuesta debe contener un token JWT
```

| | |
|---|---|
| **Severidad** | 🔴 Alta |
| **Resultado esperado** | 200 OK + token JWT |
| **Resultado obtenido** | 200 OK + token JWT |
 
**Evidencia:**
![CP-AUTH-01](evidencias/CP-AUTH-01.png)
---
 
### CP-AUTH-02 — Registro con email duplicado
```gherkin
  Scenario: Registro fallido por email ya existente
    Given que el email "test@mail.com" ya está registrado en el sistema
    When envío un POST a /api/auth/register con el body:
      | email    | test@mail.com |
      | password | 123456        |
    Then la respuesta debe tener el código 400 Bad Request
    And la respuesta debe contener el mensaje "El correo electrónico ya está registrado."
```

| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 400 Bad Request |
| **Resultado obtenido** | 400 Bad Request |

**Evidencia:**
![CP-AUTH-02](evidencias/CP-AUTH-02.png)
---
 
### CP-AUTH-03 — Registro con email inválido
```gherkin
  Scenario: Registro fallido por formato de email inválido
    Given que el endpoint POST /api/auth/register está disponible
    When envío un POST a /api/auth/register con el body:
      | email    | esto-no-es-un-email |
      | password | 123456              |
    Then la respuesta debe tener el código 400 Bad Request
    And la respuesta debe contener un error de validación de formato de email
```

| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 400 Bad Request |
| **Resultado obtenido** | 400 Bad Request |

**Evidencia:**
![CP-AUTH-03](evidencias/CP-AUTH-03.png) 

---
 
### CP-AUTH-04 — Registro con contraseña corta
```gherkin
  Scenario: Registro fallido por contraseña menor a 6 caracteres
    Given que el endpoint POST /api/auth/register está disponible
    When envío un POST a /api/auth/register con el body:
      | email    | nuevo@mail.com |
      | password | 123            |
    Then la respuesta debe tener el código 400 Bad Request
    And la respuesta debe contener el mensaje "La contraseña debe tener al menos 6 caracteres."
```

| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 400 Bad Request |
| **Resultado obtenido** | 400 Bad Request |

**Evidencia:**
![CP-AUTH-04](evidencias/CP-AUTH-04.png)  
---
 
### CP-AUTH-05 — Login exitoso
```gherkin
  Scenario: Login exitoso con credenciales válidas
    Given que el usuario "test@mail.com" está registrado en el sistema
    When envío un POST a /api/auth/login con el body:
      | email    | test@mail.com |
      | password | 123456        |
    Then la respuesta debe tener el código 200 OK
    And la respuesta debe contener un token JWT válido
```
| | |
|---|---|
| **Severidad** | 🔴 Alta |
| **Resultado esperado** | 200 OK + token JWT |
| **Resultado obtenido** | 200 OK + token JWT |

**Evidencia:**
![CP-AUTH-05](evidencias/CP-AUTH-05.png)  
---
 
### CP-AUTH-06 — Login con contraseña incorrecta
```gherkin
  Scenario: Login fallido por contraseña incorrecta
    Given que el usuario "test@mail.com" está registrado en el sistema
    When envío un POST a /api/auth/login con el body:
      | email    | test@mail.com |
      | password | wrongpass     |
    Then la respuesta debe tener el código 401 Unauthorized
    And la respuesta debe contener el mensaje "Credenciales incorrectas o usuario inactivo."
```
| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 401 Unauthorized |
| **Resultado obtenido** | 401 Unauthorized |

**Evidencia:**
![CP-AUTH-06](evidencias/CP-AUTH-06.png)  
---
 
### CP-AUTH-07 — Login con email inexistente
```gherkin
  Scenario: Login fallido por usuario no registrado
    Given que el email "noexiste@mail.com" no está registrado en el sistema
    When envío un POST a /api/auth/login con el body:
      | email    | noexiste@mail.com |
      | password | 123456            |
    Then la respuesta debe tener el código 401 Unauthorized
```
| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 401 Unauthorized |
| **Resultado obtenido** | 401 Unauthorized |

**Evidencia:**
![CP-AUTH-07](evidencias/CP-AUTH-07.png)  
---
 
### CP-AUTH-08 — Error interno al registrar usuario
```gherkin
  Scenario: Error interno del servidor al registrar
    Given que la base de datos no está disponible
    When envío un POST a /api/auth/register con el body:
      | email    | test@mail.com |
      | password | 123456        |
    Then la respuesta debe tener el código 500 Internal Server Error
    And la respuesta debe contener el mensaje "Error interno del servidor"
```
| | |
|---|---|
| **Severidad** | 🟢 Baja |
| **Resultado esperado** | 500 Internal Server Error |
| **Resultado obtenido** | 500 Internal Server Error |

**Evidencia:**
![CP-AUTH-08](evidencias/CP-AUTH-08.png) 
---
 
### CP-AUTH-09 — Error interno al hacer login
```gherkin
  Scenario: Error interno del servidor al hacer login
    Given que la base de datos no está disponible
    When envío un POST a /api/auth/login con el body:
      | email    | test@mail.com |
      | password | 123456        |
    Then la respuesta debe tener el código 500 Internal Server Error
    And la respuesta debe contener el mensaje "Error interno del servidor"
```
| | |
|---|---|
| **Severidad** | 🟢 Baja |
| **Resultado esperado** | 500 Internal Server Error |
| **Resultado obtenido** | 500 Internal Server Error |

**Evidencia:**
![CP-AUTH-09](evidencias/CP-AUTH-09.png) 
---
 
## MÓDULO 2 — CATEGORÍAS
 
### CP-CAT-01 — Listar categorías con datos
```gherkin
Feature: Gestión de categorías
 
  Scenario: Listar categorías cuando existen datos semilla
    Given que el sistema fue iniciado y cargó los datos semilla
    When envío un GET a /api/category?page=1&pageSize=10
    Then la respuesta debe tener el código 200 OK
    And la respuesta debe contener una lista de categorías
```
| | |
|---|---|
| **Severidad** | 🔴 Alta |
| **Resultado esperado** | 200 OK + lista de categorías |
| **Resultado obtenido** | 200 OK + lista de categorías |

**Evidencia:**
![CP-CAT-01](evidencias/CP-CAT-01.png)  
---
 
### CP-CAT-02 — Paginación de categorías
```gherkin
  Scenario: Listar categorías con paginación de 2 resultados
    Given que existen más de 2 categorías en el sistema
    When envío un GET a /api/category?page=1&pageSize=2
    Then la respuesta debe tener el código 404 Error: Not Found
    And la respuesta debe contener exactamente 2 categorías
```
| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 404 Error: Not Found |
| **Resultado obtenido** | 404 Error: Not Found |

**Evidencia:**
![CP-CAT-02](evidencias/CP-CAT-02.png)
---
 
### CP-CAT-03 — Obtener categoría por ID válido
```gherkin
  Scenario: Obtener una categoría existente por su ID
    Given que existe una categoría con un ID válido en el sistema
    When envío un GET a /api/category/{id}
    Then la respuesta debe tener el código 200 OK
    And la respuesta debe contener el objeto de la categoría con ese ID
```
| | |
|---|---|
| **Severidad** | 🔴 Alta |
| **Resultado esperado** | 200 OK + objeto categoría |
| **Resultado obtenido** | 200 OK + objeto categoría |

**Evidencia:**
![CP-CAT-03](evidencias/CP-CAT-03.png)
---
 
### CP-CAT-04 — Obtener categoría con ID inexistente
```gherkin
  Scenario: Obtener una categoría con ID que no existe
    Given que no existe ninguna categoría con el ID "00000000-0000-0000-0000-000000000001"
    When envío un GET a /api/category/00000000-0000-0000-0000-000000000001
    Then la respuesta debe tener el código 404 Not Found
    And la respuesta debe contener un mensaje descriptivo del error
```
| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 404 Not Found |
| **Resultado obtenido** | 404 Not Found |

**Evidencia:**
![CP-CAT-04](evidencias/CP-CAT-04.png)
---
 
### CP-CAT-05 — Crear categoría exitosamente
```gherkin
  Scenario: Crear una nueva categoría con datos válidos
    Given que el endpoint POST /api/category está disponible
    When envío un POST a /api/category con el body:
      | categoryID   | 00000000-0000-0000-0000-000000000000 |
      | nameCategory | Cables y Conectores                  |
    Then la respuesta debe tener el código 201 Created
    And la respuesta debe contener el objeto creado con un ID asignado
```
| | |
|---|---|
| **Severidad** | 🔴 Alta |
| **Resultado esperado** | 201 Created + objeto creado |
| **Resultado obtenido** | 201 Created + objeto creado |

**Evidencia:**
![CP-CAT-05](evidencias/CP-CAT-05.png)
36506a8f-9ec1-49dc-9b24-80113e5a7d65
---
 
### CP-CAT-06 — Crear categoría sin nombre
```gherkin
  Scenario: Crear una categoría sin nombre falla con error de validación
    Given que el endpoint POST /api/category está disponible
    When envío un POST a /api/category con el body:
      | categoryID   | 00000000-0000-0000-0000-000000000000 |
      | nameCategory |                                      |
    Then la respuesta debe tener el código 400 Bad Request
    And la respuesta debe contener el mensaje "El nombre de la categoría es obligatorio."
```
| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 400 Bad Request |
| **Resultado obtenido** | 400 Bad Request |

**Evidencia:**
![CP-CAT-06](evidencias/CP-CAT-06.png) 
---
 
### CP-CAT-07 — Actualizar categoría exitosamente
```gherkin
  Scenario: Actualizar una categoría existente con datos válidos
    Given que existe una categoría con un ID válido en el sistema
    When envío un PUT a /api/category/{id} con el body:
      | categoryID   | {mismo id}         |
      | nameCategory | Nombre Actualizado |
    Then la respuesta debe tener el código 204 No Content
```
| | |
|---|---|
| **Severidad** | 🔴 Alta |
| **Resultado esperado** | 204 No Content |
| **Resultado obtenido** | 204 No Content |

**Evidencia:**
![CP-CAT-07](evidencias/CP-CAT-07.png) 
---
 
### CP-CAT-08 — Actualizar con IDs que no coinciden
```gherkin
  Scenario: Actualizar una categoría con IDs inconsistentes falla
    Given que existe una categoría con un ID válido en el sistema
    When envío un PUT a /api/category/{id-real} con el body:
      | categoryID   | 00000000-0000-0000-0000-000000000099 |
      | nameCategory | Audio                                |
    Then la respuesta debe tener el código 400 Bad Request
    And la respuesta debe contener el mensaje "El ID de la URL no coincide con el ID del cuerpo de la solicitud."
```
| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 400 Bad Request |
| **Resultado obtenido** | 400 Bad Request |

**Evidencia:**
![CP-CAT-08](evidencias/CP-CAT-08.png) 
---
 
### CP-CAT-09 — Eliminar categoría exitosamente
```gherkin
  Scenario: Eliminar una categoría existente
    Given que existe una categoría creada con el ID "{id}"
    When envío un DELETE a /api/category/{id}
    Then la respuesta debe tener el código 204 No Content
```
| | |
|---|---|
| **Severidad** | 🔴 Alta |
| **Resultado esperado** | 204 No Content |
| **Resultado obtenido** | 204 No Content |

**Evidencia:**
![CP-CAT-09](evidencias/CP-CAT-09.png)  
---
 
### CP-CAT-10 — Eliminar categoría inexistente
```gherkin
  Scenario: Eliminar una categoría que no existe falla con 404
    Given que no existe ninguna categoría con el ID "00000000-0000-0000-0000-000000000001"
    When envío un DELETE a /api/category/00000000-0000-0000-0000-000000000001
    Then la respuesta debe tener el código 404 Not Found
```
| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 404 Not Found |
| **Resultado obtenido** | 404 Not Found |

**Evidencia:**
![CP-CAT-10](evidencias/CP-CAT-10.png)  
---
 
### CP-CAT-11 — Error interno al listar categorías
```gherkin
  Scenario: Error interno del servidor al listar categorías
    Given que la base de datos no está disponible
    When envío un GET a /api/category
    Then la respuesta debe tener el código 500 Internal Server Error
    And la respuesta debe contener el mensaje "Error interno del servidor"
```
| | |
|---|---|
| **Severidad** | 🟢 Baja |
| **Resultado esperado** | 500 Internal Server Error |
| **Resultado obtenido** | 500 Internal Server Error |

**Evidencia:**
![CP-CAT-11](evidencias/CP-CAT-11.png)   
---
 
### CP-CAT-12 — Error interno al crear categoría
```gherkin
  Scenario: Error interno del servidor al crear categoría
    Given que la base de datos no está disponible
    When envío un POST a /api/category con datos válidos
    Then la respuesta debe tener el código 500 Internal Server Error
    And la respuesta debe contener el mensaje "Error interno del servidor"
```
| | |
|---|---|
| **Severidad** | 🟢 Baja |
| **Resultado esperado** | 500 Internal Server Error |
| **Resultado obtenido** | 500 Internal Server Error |

**Evidencia:**
![CP-CAT-12](evidencias/CP-CAT-12.png) 
---
 
## MÓDULO 3 — ESTADOS
 
### CP-STA-01 — Listar estados con datos semilla
```gherkin
Feature: Gestión de estados
 
  Scenario: Listar estados cargados automáticamente al iniciar el sistema
    Given que el sistema fue iniciado y cargó los datos semilla
    When envío un GET a /api/status
    Then la respuesta debe tener el código 200 OK
    And la respuesta debe contener los estados: "Disponible", "En uso", "En mantenimiento", "Fuera de servicio"
```
| | |
|---|---|
| **Severidad** | 🔴 Alta |
| **Resultado esperado** | 200 OK + lista de estados |
| **Resultado obtenido** | 200 OK + lista de estados |

**Evidencia:**
![CP-STA-01](evidencias/CP-STA-01.png) 
---
 
### CP-STA-02 — Obtener estado por ID válido
```gherkin
  Scenario: Obtener un estado existente por su ID
    Given que existe un estado con un ID válido en el sistema
    When envío un GET a /api/status/{id}
    Then la respuesta debe tener el código 200 OK
    And la respuesta debe contener el objeto del estado
```
| | |
|---|---|
| **Severidad** | 🔴 Alta |
| **Resultado esperado** | 200 OK + objeto estado |
| **Resultado obtenido** | 200 OK + objeto estado |

**Evidencia:**
![CP-STA-02](evidencias/CP-STA-02.png) 
---
 
### CP-STA-03 — Crear estado exitosamente
```gherkin
  Scenario: Crear un nuevo estado con datos válidos
    Given que el endpoint POST /api/status está disponible
    When envío un POST a /api/status con el body:
      | statusId    | 00000000-0000-0000-0000-000000000000 |
      | nameStatus  | Reservado                            |
      | description | Activo reservado para un evento      |
    Then la respuesta debe tener el código 201 Created
    And la respuesta debe contener el objeto creado con un ID asignado
```
| | |
|---|---|
| **Severidad** | 🔴 Alta |
| **Resultado esperado** | 201 Created + objeto creado |
| **Resultado obtenido** | 201 Created + objeto creado |

**Evidencia:**
![CP-STA-03](evidencias/CP-STA-03.png) 
---
 
### CP-STA-04 — Crear estado sin nombre
```gherkin
  Scenario: Crear un estado sin nombre falla con error de validación
    Given que el endpoint POST /api/status está disponible
    When envío un POST a /api/status con el body:
      | statusId   | 00000000-0000-0000-0000-000000000025 |
      | nameStatus |                                      |
    Then la respuesta debe tener el código 400 Bad Request
    And la respuesta debe contener el mensaje "El nombre del estado es obligatorio."
```
| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 400 Bad Request |
| **Resultado obtenido** | 400 Bad Request |
 
**Evidencia:**
![CP-STA-04](evidencias/CP-STA-04.png) 
---
 
### CP-STA-05 — Actualizar estado exitosamente
```gherkin
  Scenario: Actualizar un estado existente con datos válidos
    Given que existe un estado con un ID válido en el sistema
    When envío un PUT a /api/status/{id} con el body:
      | statusId   | {mismo id}               |
      | nameStatus | Disponible - Actualizado |
    Then la respuesta debe tener el código 204 No Content
```
| | |
|---|---|
| **Severidad** | 🔴 Alta |
| **Resultado esperado** | 204 No Content |
| **Resultado obtenido** | 204 No Content |

**Evidencia:**
![CP-STA-05](evidencias/CP-STA-05.png) 
---
 
### CP-STA-06 — Eliminar estado exitosamente
```gherkin
  Scenario: Eliminar un estado recién creado
    Given que existe un estado con ID "{id}" creado en CP-STA-03
    When envío un DELETE a /api/status/{id}
    Then la respuesta debe tener el código 204 No Content
```
| | |
|---|---|
| **Severidad** | 🔴 Alta |
| **Resultado esperado** | 204 No Content |
| **Resultado obtenido** | 204 No Content |

**Evidencia:**
![CP-STA-06](evidencias/CP-STA-06.png)
---
 
### CP-STA-07 — Error interno al listar estados
```gherkin
  Scenario: Error interno del servidor al listar estados
    Given que la base de datos no está disponible
    When envío un GET a /api/status
    Then la respuesta debe tener el código 500 Internal Server Error
    And la respuesta debe contener el mensaje "Error interno del servidor"
```
| | |
|---|---|
| **Severidad** | 🟢 Baja |
| **Resultado esperado** | 500 Internal Server Error |
| **Resultado obtenido** | 500 Internal Server Error |

**Evidencia:**
![CP-STA-07](evidencias/CP-STA-07.png)
---
 
### CP-STA-08 — Error interno al crear estado
```gherkin
  Scenario: Error interno del servidor al crear estado
    Given que la base de datos no está disponible
    When envío un POST a /api/status con datos válidos
    Then la respuesta debe tener el código 500 Internal Server Error
    And la respuesta debe contener el mensaje "Error interno del servidor"
```
| | |
|---|---|
| **Severidad** | 🟢 Baja |
| **Resultado esperado** | 500 Internal Server Error |
| **Resultado obtenido** | 500 Internal Server Error |

**Evidencia:**
![CP-STA-08](evidencias/CP-STA-08.png) 
---
 
## MÓDULO 4 — ACTIVOS
 
### CP-ASS-01 — Listar activos sin datos
```gherkin
Feature: Gestión de activos
 
  Scenario: Listar activos cuando no hay ninguno registrado
    Given que no existen activos en el sistema
    When envío un GET a /api/asset
    Then la respuesta debe tener el código 404 Not Found
    And la respuesta debe contener el mensaje "No hay activos registrados en el sistema."
```
| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 404 Not Found |
| **Resultado obtenido** | 404 Not Found |

**Evidencia:**
![CP-ASS-01](evidencias/CP-ASS-01.png)  
---
 
### CP-ASS-02 — Crear activo exitosamente
```gherkin
  Scenario: Crear un nuevo activo con todos los datos válidos
    Given que existe una categoría con ID "{id-categoria}" en el sistema
    And que existe un estado con ID "{id-status}" en el sistema
    When envío un POST a /api/asset con el body:
      | assetId    | 00000000-0000-0000-0000-000000000000 |
      | name       | Micrófono Shure SM58                 |
      | codeId     | MIC-001                              |
      | categoryId | {id-categoria}                       |
      | statusId   | {id-status}                          |
      | isDeleted  | false                                |
    Then la respuesta debe tener el código 201 Created
    And la respuesta debe contener el objeto creado con un ID asignado
```
| | |
|---|---|
| **Severidad** | 🔴 Alta |
| **Resultado esperado** | 201 Created + objeto creado |
| **Resultado obtenido** | 201 Created + objeto creado |

**Evidencia:**
![CP-ASS-02](evidencias/CP-ASS-02.png) 
---
 
### CP-ASS-03 — Crear activo sin nombre
```gherkin
  Scenario: Crear un activo sin nombre falla con error de validación
    Given que el endpoint POST /api/asset está disponible
    When envío un POST a /api/asset con el campo name vacío
    Then la respuesta debe tener el código 400 Bad Request
    And la respuesta debe contener el mensaje "El nombre del activo es obligatorio."
```
| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 400 Bad Request |
| **Resultado obtenido** | 400 Bad Request |

**Evidencia:**
![CP-ASS-03](evidencias/CP-ASS-03.png)  
---
 
### CP-ASS-04 — Crear activo sin código
```gherkin
  Scenario: Crear un activo sin código falla con error de validación
    Given que el endpoint POST /api/asset está disponible
    When envío un POST a /api/asset con el campo codeId vacío
    Then la respuesta debe tener el código 400 Bad Request
    And la respuesta debe contener el mensaje "El código del activo es obligatorio."
```
| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 400 Bad Request |
| **Resultado obtenido** | 400 Bad Request |
 
**Evidencia:**
![CP-ASS-04](evidencias/CP-ASS-04.png)  
---
 
### CP-ASS-05 — Crear activo sin categoría
```gherkin
  Scenario: Crear un activo sin categoría falla con error de validación
    Given que el endpoint POST /api/asset está disponible
    When envío un POST a /api/asset con categoryId igual a "00000000-0000-0000-0000-000000000000"
    Then la respuesta debe tener el código 400 Bad Request
    And la respuesta debe contener el mensaje "La categoría del activo es obligatoria."
```
| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 400 Bad Request |
| **Resultado obtenido** | 400 Bad Request |

**Evidencia:**
![CP-ASS-05](evidencias/CP-ASS-05.png) 
---
 
### CP-ASS-06 — Listar activos paginados
```gherkin
  Scenario: Listar activos con paginación de 1 resultado por página
    Given que existen al menos 2 activos en el sistema
    When envío un GET a /api/asset?page=1&pageSize=1
    Then la respuesta debe tener el código 200 OK
    And la respuesta debe contener exactamente 1 activo
```
| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 404 Error: Not Found |
| **Resultado obtenido** | 404 Error: Not Found |
 
**Evidencia:**
![CP-ASS-06](evidencias/CP-ASS-06.png) 
---
 
### CP-ASS-07 — Obtener activo por ID válido
```gherkin
  Scenario: Obtener un activo existente por su ID
    Given que existe un activo con ID "{id}" en el sistema
    When envío un GET a /api/asset/{id}
    Then la respuesta debe tener el código 200 OK
    And la respuesta debe contener el objeto del activo con ese ID
```
| | |
|---|---|
| **Severidad** | 🔴 Alta |
| **Resultado esperado** | 200 OK + objeto activo |
| **Resultado obtenido** | 200 OK + objeto activo |

**Evidencia:**
![CP-ASS-07](evidencias/CP-ASS-07.png)  
---
 
### CP-ASS-08 — Actualizar activo exitosamente
```gherkin
  Scenario: Actualizar un activo existente con datos válidos
    Given que existe un activo con ID "{id}" en el sistema
    When envío un PUT a /api/asset/{id} con el body:
      | assetId    | {mismo id}            |
      | name       | Micrófono Actualizado |
      | codeId     | MIC-001               |
      | categoryId | {id-categoria}        |
      | statusId   | {id-status}           |
    Then la respuesta debe tener el código 204 No Content
```
| | |
|---|---|
| **Severidad** | 🔴 Alta |
| **Resultado esperado** | 204 No Content |
| **Resultado obtenido** | 204 No Content |

**Evidencia:**
![CP-ASS-08](evidencias/CP-ASS-08.png)  
---
 
### CP-ASS-09 — Eliminar activo (soft delete)
```gherkin
  Scenario: Eliminar un activo aplica soft delete
    Given que existe un activo con ID "{id}" en el sistema
    When envío un DELETE a /api/asset/{id}
    Then la respuesta debe tener el código 204 No Content
```
| | |
|---|---|
| **Severidad** | 🔴 Alta |
| **Resultado esperado** | 204 No Content |
| **Resultado obtenido** | 204 No Content |

**Evidencia:**
![CP-ASS-09](evidencias/CP-ASS-09.png)  
---
 
### CP-ASS-10 — Verificar soft delete
```gherkin
  Scenario: Un activo eliminado no aparece en las consultas
    Given que el activo con ID "{id}" fue eliminado en CP-ASS-09
    When envío un GET a /api/asset/{id}
    Then la respuesta debe tener el código 404 Not Found
```
| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 404 Not Found |
| **Resultado obtenido** | 404 Not Found |

**Evidencia:**
![CP-ASS-10](evidencias/CP-ASS-10.png)   
---
 
### CP-ASS-11 — Error interno al listar activos
```gherkin
  Scenario: Error interno del servidor al listar activos
    Given que la base de datos no está disponible
    When envío un GET a /api/asset
    Then la respuesta debe tener el código 500 Internal Server Error
    And la respuesta debe contener el mensaje "Error interno del servidor"
```
| | |
|---|---|
| **Severidad** | 🟢 Baja |
| **Resultado esperado** | 500 Internal Server Error |
| **Resultado obtenido** | 500 Internal Server Error |

**Evidencia:**
![CP-ASS-11](evidencias/CP-ASS-11.png)  
---
 
### CP-ASS-12 — Error interno al crear activo
```gherkin
  Scenario: Error interno del servidor al crear activo
    Given que la base de datos no está disponible
    When envío un POST a /api/asset con datos válidos
    Then la respuesta debe tener el código 500 Internal Server Error
    And la respuesta debe contener el mensaje "Error interno del servidor"
```
| | |
|---|---|
| **Severidad** | 🟢 Baja |
| **Resultado esperado** | 500 Internal Server Error |
| **Resultado obtenido** | 500 Internal Server Error |

**Evidencia:**
![CP-ASS-12](evidencias/CP-ASS-12.png) 
---
 
## MÓDULO 5 — CLIENTES
 
### CP-CLI-01 — Crear cliente exitosamente
```gherkin
Feature: Gestión de clientes
 
  Scenario: Crear un nuevo cliente con datos válidos
    Given que el endpoint POST /api/clients está disponible
    When envío un POST a /api/clients con el body:
      | clientID | 00000000-0000-0000-0000-000000000000 |
      | name     | Juan Pérez                           |
      | dniCuit  | 20-12345678-9                        |
      | phone    | 1123456789                           |
      | isActive | true                                 |
    Then la respuesta debe tener el código 201 Created
    And la respuesta debe contener el objeto creado con un ID asignado
```
| | |
|---|---|
| **Severidad** | 🔴 Alta |
| **Resultado esperado** | 201 Created + objeto creado |
| **Resultado obtenido** | 201 Created + objeto creado |

**Evidencia:**
![CP-CLI-01](evidencias/CP-CLI-01.png)  
---
 
### CP-CLI-02 — Crear cliente sin nombre
```gherkin
  Scenario: Crear un cliente sin nombre falla con error de validación
    Given que el endpoint POST /api/clients está disponible
    When envío un POST a /api/clients con el campo name vacío
    Then la respuesta debe tener el código 400 Bad Request
    And la respuesta debe contener el mensaje "El nombre del cliente es obligatorio."
```
| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 400 Bad Request |
| **Resultado obtenido** | 400 Bad Request |

**Evidencia:**
![CP-CLI-02](evidencias/CP-CLI-02.png)  
---
 
### CP-CLI-03 — Crear cliente sin DNI/CUIT
```gherkin
  Scenario: Crear un cliente sin DNI/CUIT falla con error de validación
    Given que el endpoint POST /api/clients está disponible
    When envío un POST a /api/clients con el campo dniCuit vacío
    Then la respuesta debe tener el código 400 Bad Request
    And la respuesta debe contener el mensaje "El DNI/CUIT del cliente es obligatorio."
```
| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 400 Bad Request |
| **Resultado obtenido** | 400 Bad Request |

**Evidencia:**
![CP-CLI-03](evidencias/CP-CLI-03.png)   
---
 
### CP-CLI-04 — Listar clientes activos
```gherkin
  Scenario: Listar clientes activos en el sistema
    Given que existe al menos un cliente activo en el sistema
    When envío un GET a /api/clients
    Then la respuesta debe tener el código 200 OK
    And la respuesta debe contener una lista de clientes activos
```
| | |
|---|---|
| **Severidad** | 🔴 Alta |
| **Resultado esperado** | 200 OK + lista de clientes |
| **Resultado obtenido** | 200 OK + lista de clientes |

**Evidencia:**
![CP-CLI-04](evidencias/CP-CLI-04.png)   
---
 
### CP-CLI-05 — Eliminar cliente (soft delete)
```gherkin
  Scenario: Eliminar un cliente aplica soft delete
    Given que existe un cliente con ID "{id}" en el sistema
    When envío un DELETE a /api/clients/{id}
    Then la respuesta debe tener el código 204 No Content
```
| | |
|---|---|
| **Severidad** | 🔴 Alta |
| **Resultado esperado** | 204 No Content |
| **Resultado obtenido** | 204 No Content |

**Evidencia:**
![CP-CLI-05](evidencias/CP-CLI-05.png)    
---
 
### CP-CLI-06 — Error interno al listar clientes
```gherkin
  Scenario: Error interno del servidor al listar clientes
    Given que la base de datos no está disponible
    When envío un GET a /api/clients
    Then la respuesta debe tener el código 500 Internal Server Error
    And la respuesta debe contener el mensaje "Error interno del servidor"
```
| | |
|---|---|
| **Severidad** | 🟢 Baja |
| **Resultado esperado** | 500 Internal Server Error |
| **Resultado obtenido** | 500 Internal Server Error |

**Evidencia:**
![CP-CLI-06](evidencias/CP-CLI-06.png)     
---
 
### CP-CLI-07 — Error interno al crear cliente
```gherkin
  Scenario: Error interno del servidor al crear cliente
    Given que la base de datos no está disponible
    When envío un POST a /api/clients con datos válidos
    Then la respuesta debe tener el código 500 Internal Server Error
    And la respuesta debe contener el mensaje "Error interno del servidor"
```
| | |
|---|---|
| **Severidad** | 🟢 Baja |
| **Resultado esperado** | 500 Internal Server Error |
| **Resultado obtenido** | 500 Internal Server Error |

**Evidencia:**
![CP-CLI-07](evidencias/CP-CLI-07.png)  
---
 
## MÓDULO 6 — USUARIOS
 
### CP-USR-01 — Listar usuarios sin datos
```gherkin
Feature: Gestión de usuarios
 
  Scenario: Listar usuarios cuando no hay ninguno registrado
    Given que no existen usuarios en el sistema
    When envío un GET a /api/users
    Then la respuesta debe tener el código 404 Not Found
    And la respuesta debe contener el mensaje "No hay usuarios registrados en el sistema."
```
| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 404 Not Found |
| **Resultado obtenido** | 404 Not Found |

**Evidencia:**
![CP-USR-01](evidencias/CP-USR-01.png)   
---
 
### CP-USR-02 — Obtener usuario con ID inexistente
```gherkin
  Scenario: Obtener un usuario que no existe devuelve 404
    Given que no existe ningún usuario con el ID "00000000-0000-0000-0000-000000000001"
    When envío un GET a /api/users/00000000-0000-0000-0000-000000000001
    Then la respuesta debe tener el código 404 Not Found
```
| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 404 Not Found |
| **Resultado obtenido** | 404 Not Found |

**Evidencia:**
![CP-USR-02](evidencias/CP-USR-02.png)  
---
 
### CP-USR-03 — Eliminar usuario inexistente
```gherkin
  Scenario: Eliminar un usuario que no existe devuelve 404
    Given que no existe ningún usuario con el ID "00000000-0000-0000-0000-000000000001"
    When envío un DELETE a /api/users/00000000-0000-0000-0000-000000000001
    Then la respuesta debe tener el código 404 Not Found
```
| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 404 Not Found |
| **Resultado obtenido** | 404 Not Found |

**Evidencia:**
![CP-USR-03](evidencias/CP-USR-03.png) 
---
 
### CP-USR-04 — Error interno al listar usuarios
```gherkin
  Scenario: Error interno del servidor al listar usuarios
    Given que la base de datos no está disponible
    When envío un GET a /api/users
    Then la respuesta debe tener el código 500 Internal Server Error
    And la respuesta debe contener el mensaje "Error interno del servidor"
```
| | |
|---|---|
| **Severidad** | 🟢 Baja |
| **Resultado esperado** | 500 Internal Server Error |
| **Resultado obtenido** | 500 Internal Server Error |

**Evidencia:**
![CP-USR-04](evidencias/CP-USR-04.png)  
---
 
## MÓDULO 7 — ALQUILERES
 
### CP-REN-01 — Listar alquileres vacíos
```gherkin
Feature: Gestión de alquileres
 
  Scenario: Listar alquileres cuando no hay ninguno registrado
    Given que no existen alquileres en el sistema
    When envío un GET a /api/rentals
    Then la respuesta debe tener el código 200 OK
    And la respuesta debe contener una lista vacía
```
| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 200 OK + lista vacía |
| **Resultado obtenido** | 200 OK + lista vacía |

**Evidencia:**
![CP-REN-01](evidencias/CP-REN-01.png)   
---
 
### CP-REN-02 — Crear alquiler exitosamente
```gherkin
  Scenario: Crear un nuevo alquiler con todos los datos válidos
    Given que existe un cliente con ID "{id-cliente}" en el sistema
    And que existe un usuario con ID "{id-usuario}" en el sistema
    And que existe un estado con ID "{id-status}" en el sistema
    When envío un POST a /api/rentals con el body:
      | rentalID           | 00000000-0000-0000-0000-000000000000 |
      | statusID           | {id-status}                          |
      | clientID           | {id-cliente}                         |
      | userID             | {id-usuario}                         |
      | rentalDate         | 2026-06-21T00:00:00Z                 |
      | rentalDateExpected | 2026-06-28T00:00:00Z                 |
    Then la respuesta debe tener el código 201 Created
    And la respuesta debe contener el objeto del alquiler creado con su ID
```
| | |
|---|---|
| **Severidad** | 🔴 Alta |
| **Resultado esperado** | 201 Created + objeto alquiler |
| **Resultado obtenido** | 201 Created + objeto alquiler |

**Evidencia:**
![CP-REN-02](evidencias/CP-REN-02.png)
---
 
### CP-REN-03 — Crear alquiler con body vacío
```gherkin
  Scenario: Crear un alquiler con body nulo o vacío falla
    Given que el endpoint POST /api/rentals está disponible
    When envío un POST a /api/rentals con el body vacío
    Then la respuesta debe tener el código 400 Bad Request
```
| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 400 Bad Request |
| **Resultado obtenido** | 400 Bad Request |

**Evidencia:**
![CP-REN-03](evidencias/CP-REN-03.png) 
---
 
### CP-REN-04 — Obtener alquiler por ID inexistente
```gherkin
  Scenario: Obtener un alquiler que no existe devuelve 404
    Given que no existe ningún alquiler con el ID "00000000-0000-0000-0000-000000000001"
    When envío un GET a /api/rentals/00000000-0000-0000-0000-000000000001
    Then la respuesta debe tener el código 404 Not Found
    And la respuesta debe contener el mensaje "No se encontró el alquiler con ID..."
```
| | |
|---|---|
| **Severidad** | 🟡 Media |
| **Resultado esperado** | 404 Not Found |
| **Resultado obtenido** | 404 Not Found |
 
**Evidencia:**
![CP-REN-04](evidencias/CP-REN-04.png) 
---
 
### CP-REN-05 — Error interno al listar alquileres
```gherkin
  Scenario: Error interno del servidor al listar alquileres
    Given que la base de datos no está disponible
    When envío un GET a /api/rentals
    Then la respuesta debe tener el código 500 Internal Server Error
    And la respuesta debe contener el mensaje "Error interno al recuperar los alquileres."
```
| | |
|---|---|
| **Severidad** | 🟢 Baja |
| **Resultado esperado** | 500 Internal Server Error |
| **Resultado obtenido** | 500 Internal Server Error |

**Evidencia:**
![CP-REN-05](evidencias/CP-REN-05.png)  
---
 
### CP-REN-06 — Error interno al crear alquiler
```gherkin
  Scenario: Error interno del servidor al crear alquiler
    Given que la base de datos no está disponible
    When envío un POST a /api/rentals con datos válidos
    Then la respuesta debe tener el código 500 Internal Server Error
    And la respuesta debe contener el mensaje "Error interno al crear el alquiler."
```
| | |
|---|---|
| **Severidad** | 🟢 Baja |
| **Resultado esperado** | 500 Internal Server Error |
| **Resultado obtenido** | 500 Internal Server Error |

**Evidencia:**
![CP-REN-06](evidencias/CP-REN-06.png)  
---

## 🐛 BUGS ENCONTRADOS DURANTE LAS PRUEBAS

Esta sección documenta los defectos detectados durante la ejecución de los casos de prueba manuales.

### BUG-001 — Error al actualizar entidades (PUT)

| Campo | Detalle |
|---|---|
| **ID** | BUG-001 |
| **Endpoints afectados** | `PUT /api/category/{id}`, `PUT /api/status/{id}`, `PUT /api/asset/{id}` |
| **Casos de prueba** | CP-CAT-07, CP-STA-05, CP-ASS-08 |
| **Severidad** | Alta |
| **Estado** | ✅ **Resuelto** |
| **Reportado en** | 21/06/2026 |
| **Resuelto en** | 22/06/2026 |

**Descripción:**
Los endpoints PUT de los controladores Category, Status y Asset devolvían error 500 Internal Server Error al intentar actualizar una entidad existente.

**Pasos para reproducir (antes del fix):**
1. Obtener un ID válido con un GET (ej: `GET /api/category`, `GET /api/status` o `GET /api/asset`)
2. Ejecutar el PUT correspondiente con el mismo ID en URL y body
3. Observar la respuesta

**Resultado esperado:**
204 No Content (entidad actualizada correctamente)

**Resultado obtenido (antes del fix):**
500 Internal Server Error con mensaje similar al siguiente:
> "The instance of entity type 'XXX' cannot be tracked because another instance with the same key value is already being tracked."

**Causa raíz:**
En los métodos `Put` de los controladores se llamaba primero a `GetByIdAsync(id)` para verificar que la entidad existiera. Ese método cargaba la entidad en el contexto de Entity Framework y la dejaba siendo rastreada. Luego, al ejecutar `UpdateAsync(id, entidad)` con una nueva instancia que tenía el mismo ID, Entity Framework detectaba el conflicto de tracking porque no puede rastrear dos instancias de la misma entidad simultáneamente.

**Solución aplicada:**
Se eliminó la verificación previa con `GetByIdAsync` en los tres controladores (CategoryController, StatusController y AssetController). En su lugar, se delegó la responsabilidad de detectar la existencia al método `UpdateAsync` del servicio, que devuelve `true` si la actualización fue exitosa o `false` si la entidad no existe.

**Cambios realizados:**

**CategoryController.cs — método PutCategory**
- Se eliminó:
```csharp
var existe = await _categoryService.GetByIdAsync(id);
if (existe == null)
    return NotFound(...);
```
- Se dejó la llamada directa a `UpdateAsync` y se evalúa el resultado para devolver 404 si la entidad no existe.

**StatusController.cs — método PutStatus**
- Se aplicó el mismo patrón eliminando la verificación previa con `GetByIdAsync`.

**AssetController.cs — método PutAsset**
- Se aplicó el mismo patrón eliminando la verificación previa con `GetByIdAsync`.

**Flujo corregido:**
1. Llega request PUT con ID y body
2. Validar campos (ID, nombre, código, etc)
3. Llamar a `UpdateAsync(id, entidad)` directamente
4. Si el método devuelve `false` → responder 404 Not Found
5. Si el método devuelve `true` → responder 204 No Content

**Beneficios adicionales:**
- Se reduce de 2 a 1 las llamadas a la base de datos por cada PUT
- Se elimina el conflicto de tracking de Entity Framework
- Mejora el rendimiento de los endpoints de actualización

**Verificación:**
Se ejecutaron nuevamente los casos CP-CAT-07, CP-STA-05 y CP-ASS-08 obteniendo el código 204 No Content esperado en los tres controladores.

---

### Resumen de bugs

| ID | Endpoints | Severidad | Estado |
|---|---|---|---|
| BUG-001 | PUT /api/category/{id}, PUT /api/status/{id}, PUT /api/asset/{id} | Alta | ✅ Resuelto |
---


## RESUMEN DE CASOS POR CÓDIGO HTTP
 
| Módulo | Total casos | 200/201 | 204 | 400 | 401 | 404 | 500 |
|---|---|---|---|---|---|---|---|
| Autenticación | 9 | 2 | - | 3 | 2 | - | 2 |
| Categorías | 12 | 3 | 2 | 2 | - | 2 | 2 |
| Estados | 8 | 2 | 2 | 1 | - | - | 2 |
| Activos | 12 | 3 | 2 | 3 | - | 2 | 2 |
| Clientes | 7 | 2 | 1 | 2 | - | - | 2 |
| Usuarios | 4 | - | - | - | - | 2 | 1 |
| Alquileres | 6 | 2 | - | 1 | - | 1 | 2 |
| **Total** | **58** | **14** | **7** | **12** | **2** | **7** | **13** |

---

## RESUMEN DE CASOS POR SEVERIDAD

| Módulo | Total | 🔴 Alta | 🟡 Media | 🟢 Baja |
|---|---|---|---|---|
| Autenticación | 9 | 2 | 5 | 2 |
| Categorías | 12 | 5 | 5 | 2 |
| Estados | 8 | 5 | 1 | 2 |
| Activos | 12 | 5 | 5 | 2 |
| Clientes | 7 | 3 | 2 | 2 |
| Usuarios | 4 | - | 3 | 1 |
| Alquileres | 6 | 1 | 3 | 2 |
| **Total** | **58** | **21** | **24** | **13** |
 
---
 
## ORDEN RECOMENDADO DE EJECUCIÓN
 
```
1. CP-AUTH-01 → Registrar usuario
2. CP-AUTH-05 → Login y obtener token JWT
3. CP-STA-01  → Verificar estados semilla
4. CP-CAT-01  → Verificar categorías semilla
5. CP-ASS-02  → Crear activo usando IDs de status y category
6. CP-CLI-01  → Crear cliente
7. CP-REN-02  → Crear alquiler usando IDs anteriores
8. Casos de error 400 y 404 por módulo
9. Casos de error 500 (apagar BD o cambiar contraseña en appsettings.json)
```