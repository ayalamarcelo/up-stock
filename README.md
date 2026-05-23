<h1 align="center">Estructura de los Endpoints</h1>

<br>

<h2>Usuarios</h2>

### _*1. Users Endpoint (/users)*_


|  Method  |   Endpoint  |               Action                    |
|----------|-------------|-----------------------------------------|
|  GET     | /users      | Obtener a todos los usuarios            |
|  GET     | /users/:id  | Obtener un usuario por id               |
|  POST    | /users      | Crear un nuevo usuario                  |
|  PUT     | /users/:id  | Actualizar la información de un usuario |
|  DELETE  | /users/:id  | Eliminar un usuario por id              |

<br>

#

### _*2. Assets Endpoint (/assets o items)*_


|  Method  |   Endpoint  |               Action                    |
|----------|-------------|-----------------------------------------|
|  GET     | /assets     | Listar todos los activos o items        |
|  GET     | /users/:id  | Obtener detalles de un único item o     |
|  POST    | /users      | registrar un nuevo item o activo        |
|  PUT     | /users/:id  | Actualizar información                  |
|  DELETE  | /users/:id  | Eliminar un item o activo del sistema   |

<br>

#

### _*3. Rentals Endpoints (/rentals o /rent)*_

El estándar para cambiar el esto es un usar un POST, porque estaríamos creando una nueva transacción de alquiler.

POST/rentals o /rent

Cuerpo de la petición (payload)

```json
{
    "userId": "string/uuid",
    "assetId": "string/uuid",
    "rentalDate": "datetime"
}
```
**uuid**: _Universal unique identifier, estándar de identificación diseñado para que cada ID generado sea único. En la base de datos en lugar de `INT` usamos el tipo de dato `UUID` en postgreSQL._


1. Verificar que el `userId` existe y que el `assetId` tiene status de disponible `available`.
2. Cambiar el status del asset de `available` a `rented` o `occupied`.
3. Guardar la relación en una tabla o array de `rentals`.
4. Devolver un `201` con los detalles del movimiento.

<br>

## Correr proyecto

1. `cd backend`
2. `dotnet restore`
3. `dotnet run`
4. `dotnet ef database update --framework net9.0` | Solo si el paso 3. no les funciona, eligen la versión de .NET que tienen
5. corren este puerto, está en el backend http: `http://localhost:5102/swagger`
