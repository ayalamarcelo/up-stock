# Testing Manual

## Proyecto

UpStock - Sistema de Gestión de Alquiler y Control de Activos

## Objetivo

El objetivo de estas pruebas fue verificar el correcto funcionamiento de los endpoints expuestos por la API mediante Swagger, validando operaciones de consulta, creación, modificación y eliminación de registros, así como el manejo de errores y validaciones.

---

# Entorno de pruebas

## Herramientas utilizadas

* Swagger UI
* ASP.NET Core Web API
* PostgreSQL
* pgAdmin 4
* Visual Studio

## Configuración realizada

Durante las pruebas se detectó inicialmente un error relacionado con la cadena de conexión de la base de datos:

"The ConnectionString property has not been initialized."

Para resolverlo fue necesario:

1. Crear el archivo `appsettings.Development.json`.
2. Configurar la cadena de conexión a PostgreSQL.
3. Instalar PostgreSQL.
4. Crear la base de datos utilizada por el proyecto.
5. Ejecutar nuevamente la aplicación.

Una vez realizada la configuración, la API pudo conectarse correctamente a la base de datos y comenzaron a obtenerse respuestas válidas desde los endpoints.

---

# Resultados de las pruebas

## Módulo Asset

### GET /api/Asset

Resultado obtenido: 404 Not Found

Respuesta: "No hay activos registrados en el sistema."

Estado: Correcto.

Observación: El endpoint funciona correctamente y devuelve un mensaje informativo cuando no existen registros.

### GET /api/Asset/{id}

Resultado obtenido: 404 Not Found

Respuesta: "No se encontró ningún activo con el ID solicitado."

Estado: Correcto.

Observación: El endpoint respondió correctamente indicando que no existe un activo asociado al ID consultado.

### POST /api/Asset

Resultado obtenido: 500 Internal Server Error

Respuesta: "An error occurred while saving the entity changes."

Estado: Fallido.

Observación: El endpoint intenta guardar información pero ocurre un error durante la persistencia de datos.

### PUT /api/Asset/{id}

Resultado obtenido: 400 Bad Request

Respuesta: "The JSON value could not be converted to System.Guid."

Estado: Correcto.

Observación: La validación funciona correctamente cuando se envía un identificador inválido.

### DELETE /api/Asset/{id}

Resultado obtenido: 404 Not Found

Respuesta: "No se encontró ningún activo con el ID solicitado."

Estado: Correcto.

---

# Módulo Category

### GET /api/Category

Resultado obtenido: 200 OK

Estado: Correcto.

Observación: Se recuperó correctamente la información almacenada en la tabla Category.

### GET /api/Category/{id}

Resultado obtenido: 404 Not Found

Estado: Correcto.

Observación: El identificador utilizado no existía en la base de datos.

### POST /api/Category

Resultado obtenido: 500 Internal Server Error

Respuesta: "An error occurred while saving the entity changes."

Estado: Fallido.

Observación: El endpoint recibe correctamente la solicitud, pero se produce un error al intentar guardar los datos en la base de datos. Es necesario revisar la configuración de la entidad, las restricciones de la tabla o las relaciones asociadas.

### PUT /api/Category/{id}

Resultado obtenido:

404 Not Found

Estado: Correcto.

Observación: El endpoint respondió correctamente indicando que no existe ninguna categoría asociada al ID enviado. Esto demuestra que la validación de existencia del registro funciona adecuadamente.

### DELETE /api/Category/{id}

Resultado obtenido:

404 Not Found

Estado: Correcto.

Observación: El endpoint respondió correctamente informando que no existe ninguna categoría con el ID especificado. El comportamiento es el esperado cuando se intenta eliminar un registro inexistente.

---

# Módulo Clients

### GET /api/Clients

Resultado obtenido: 500 Internal Server Error

Respuesta: '42P01: no existe la relación "Clients"'

Estado: Fallido.

### POST /api/Clients

Resultado obtenido: 500 Internal Server Error

Estado: Fallido.

### GET /api/Clients/{id}

Resultado obtenido: 500 Internal Server Error

Estado: Fallido.

### PUT /api/Clients/{id}

Resultado obtenido: 500 Internal Server Error

Estado: Fallido.

### DELETE /api/Clients/{id}

Resultado obtenido: 500 Internal Server Error

Estado: Fallido.

### Observación general

Todos los endpoints de Clients presentan errores debido a que la tabla "Clients" no existe en la base de datos PostgreSQL.

---

# Módulo Rentals

### GET /api/Rentals

Resultado obtenido: 500 Internal Server Error

Respuesta: '42P01: no existe la relación "Clients"'

Estado: Fallido.

### POST /api/Rentals

Resultado obtenido: 500 Internal Server Error

Respuesta:

"An error occurred while saving the entity changes."

Estado: Fallido.

### GET /api/Rentals/{id}

Resultado obtenido: 500 Internal Server Error

Respuesta: '42P01: no existe la relación "Clients"'

Estado: Fallido.

### Observación general

El módulo Rentals depende de la entidad Clients. Debido a la ausencia de dicha tabla en la base de datos, las operaciones no pueden ejecutarse correctamente.

---

# Módulo Status

### GET /api/Status

Resultado obtenido: 404 Not Found

Respuesta: "No hay estados registrados en el sistema."

Estado: Correcto.

Observación: El endpoint funciona correctamente devolviendo un mensaje informativo cuando no existen estados registrados en la base de datos.

### POST /api/Status

Resultado obtenido: 201 Created

Estado: Correcto.

Observación: El registro fue creado correctamente y el sistema devolvió un identificador válido.

### GET /api/Status/{id}

Resultado obtenido: 200 OK

Estado: Correcto.

Observación: La consulta utilizando el ID generado previamente devolvió el registro esperado.

### PUT /api/Status/{id}

Resultado obtenido: 400 Bad Request

Respuesta: "El ID de la URL no coincide con el ID del cuerpo de la solicitud."

Estado: Correcto.

Observación: La validación implementada funciona correctamente.

### DELETE /api/Status/{id}

Resultado obtenido: 404 Not Found

Estado: Correcto.

Observación: El identificador utilizado no fue encontrado en la base de datos.

---

# Módulo Users

### GET /api/Users

Resultado obtenido: 500 Internal Server Error

Respuesta: 42P01: no existe la relación "Users"

Estado: Fallido.

### POST /api/Users

Resultado obtenido: 500 Internal Server Error

Estado: Fallido.

### GET /api/Users/{id}

Resultado obtenido: 500 Internal Server Error

Estado: Fallido.

### PUT /api/Users/{id}

Resultado obtenido: 500 Internal Server Error

Estado: Fallido.

### DELETE /api/Users/{id}

Resultado obtenido: 500 Internal Server Error

Estado: Fallido.

### Observación general

Todos los endpoints del módulo Users presentan errores debido a que la tabla "Users" no existe actualmente en la base de datos PostgreSQL.

---

# Conclusiones

Inicialmente se resolvieron los errores de conexión mediante la configuración de PostgreSQL y la creación del archivo appsettings.Development.json.

Posteriormente se comprobó que los módulos Status y Category responden correctamente en operaciones de consulta y validación.

Se detectaron problemas de persistencia en algunos endpoints de Asset y Category durante las operaciones de creación.

Asimismo, los módulos Users y Clients presentan errores debido a la ausencia de las tablas correspondientes en la base de datos, afectando también el funcionamiento del módulo Rentals.

Se recomienda revisar las migraciones de Entity Framework, verificar la creación de tablas faltantes y validar las relaciones entre entidades para lograr el correcto funcionamiento de todos los endpoints.
