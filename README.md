## 1. Descripción del Proyecto
Solución Full Stack para la gestión del ciclo de vida de activos en empresas de eventos. El sistema centraliza el control de existencias y permite el monitoreo logístico de unidades en tiempo real mediante un sistema de estados operativos.

## 2. Especificaciones Técnicas

### Backend (Lógica y Persistencia)
* **Gestión de Estados:** Implementación de transiciones de estado obligatorias:
    * En Depósito (Disponible)
    * Alquilado
    * En Uso (Evento Activo)
    * Baja / Roto
* **Modelado de Datos:** Clasificación por tipos (Mobiliario, Iluminación, Sonido) y registro de incidencias técnicas.
* **API de Reportes:** Endpoints de cálculo de stock en tiempo real para métricas ejecutivas.

### Frontend (Visualización y Gestión)
* **Dashboard:** Indicadores clave de rendimiento (KPIs) sobre el volumen de stock total frente a activos externos.
* **Administración:** Tablas dinámicas con filtrado por tipo y estado de activo.
* **Operativa:** Formularios de actualización de estado para cambios masivos y trazabilidad de lotes.

## 3. Reglas de Negocio y Criterios de Aceptación
* **Sincronización:** Actualización automática del panel de control ante cambios en la base de datos.
* **Restricciones de Disponibilidad:** Los activos bajo el estado "Roto" o "Alquilado" quedan automáticamente excluidos de la disponibilidad para nuevos eventos.

## 4. Estructura del Repositorio
* `/backend`: Servidor de API, lógica de negocio y acceso a datos (Node.js/TS).
* `/frontend`: Interfaz de usuario y componentes de visualización (Vite/Angular).

## 5. Instrucciones de Implementación

### Requisitos Previos
* Node.js (v18+)
* pnpm (Gestor de paquetes recomendado)

### Configuración del Entorno
1. Clonar repositorio: `git clone [URL_DEL_REPO]`
2. Instalación de dependencias (en `/backend` y `/frontend`): `pnpm install`
3. Ejecución en modo desarrollo: `pnpm dev`
