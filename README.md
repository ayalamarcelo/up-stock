# Sistema de Control de Inventario y Trazabilidad de Activos

## Descripción del Proyecto
Solución **Full Stack** integral diseñada para la gestión del ciclo de vida de activos en empresas de organización de eventos. El sistema permite centralizar el control de existencias y monitorear la logística de cada unidad en tiempo real mediante un flujo dinámico de estados operativos.

El objetivo es transformar la gestión de inventario en información estratégica, facilitando la toma de decisiones mediante visualizaciones ejecutivas del estado físico y logístico de los equipos.

## Especificaciones Técnicas

### Backend (Lógica y Persistencia)
* **Gestión de Estados:** Implementación de un sistema de trazabilidad con transiciones obligatorias:
    * `En Depósito` (Disponible)
    * `Alquilado`
    * `En Uso` (Evento Activo)
    * `Baja / Roto`
* **Modelado de Datos:** Estructura que soporta la categorización por tipos (Mobiliario, Iluminación, Sonido, etc.) y un registro histórico de daños o incidencias.
* **API de Reportes:** Endpoints desarrollados para el cálculo de existencias en tiempo real que alimentan las métricas del sistema.

### Frontend (Dashboard y Visualización)
* **Panel de Control (Dashboard):** Interfaz intuitiva con indicadores clave (KPIs) que muestran el volumen total de stock frente a los activos externos.
* **Vistas de Inventario:** Tablas dinámicas con filtros por tipo de activo y estado, permitiendo identificar rápidamente equipos disponibles o en reparación.
* **Interfaz de Gestión:** Formularios optimizados para la actualización rápida de estados y gestión de lotes.


## Reglas de Negocio y Criterios de Aceptación
* **Sincronización:** Actualización automática de los indicadores del dashboard ante cualquier cambio de estado en la base de datos.
* **Restricciones de Disponibilidad:** Los activos marcados como `Roto` o `Alquilado` quedan automáticamente excluidos de la disponibilidad para nuevos eventos.
* **Segmentación:** El sistema garantiza una visualización filtrada para una gestión operativa más ágil.

## Estructura del Repositorio
```text
├── /backend    # Node.js/TS API
├── /frontend   # Vite/Angular UI
├── docker-compose.yml
└── README.md
```

### Requisitos Previos
* Node.js (v18+)
* pnpm (Gestor de paquetes recomendado)
* Docker desktop
* Docker compose
