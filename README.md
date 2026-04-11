# Proyecto: Sistema de Control de Inventario y Trazabilidad de Activos

## 1. Contexto del Proyecto
Una empresa dedicada a la organización de eventos requiere una **solución integral (Full Stack)** para gestionar el ciclo de vida de sus activos. El objetivo principal es centralizar el control de stock y conocer el estado físico y logístico de cada ítem en tiempo real.

## 2. Descripción de la Tarea
Desarrollar una aplicación que permita al cliente monitorear el inventario a través de diferentes estados operativos. El sistema debe clasificar los activos y presentar la información de manera visual y ejecutiva para facilitar la toma de decisiones.

---

## 3. Stack Tecnológico (Tecnologías)

#### Frontend (Capa de Usuario)
* **Framework:** Angular o React (a elección del desarrollador).
* **Lenguaje:** TypeScript / JavaScript.
* **Estilos:** CSS3 / SASS o frameworks de UI (Tailwind CSS, Material UI).

#### Backend (Lógica de Negocio)
* **Entorno de Ejecución:** Node.js.
* **Lenguaje:** **TypeScript** (para garantizar robustez y tipado estático).
* **Framework:** NestJS o Express.

#### Base de Datos y Persistencia
* **Motor:** **PostgreSQL** (Base de datos relacional para trazabilidad e integridad).
* **ORM:** Prisma o TypeORM (para comunicación eficiente con la DB).

#### Infraestructura y Despliegue
* **Contenedores:** **Docker** (Dockerización de la aplicación y la base de datos para entornos consistentes).
* **Orquestación:** Docker Compose.

---

## 4. Especificaciones del Requerimiento

#### A. Capa de Backend (Lógica y Persistencia)
* **Gestión de Estados:** Implementar un sistema de trazabilidad: *En Depósito, Alquilado, En Uso y Baja/Roto*.
* **Modelado de Datos:** Categorización por tipos (mobiliario, iluminación, sonido) y registro histórico de incidencias.
* **API de Reportes:** Endpoints de cálculo en tiempo real para métricas y KPIs.

#### B. Capa de Frontend (Dashboard y Visualización)
* **Panel de Control:** Indicadores visuales de stock total vs. activos fuera de depósito.
* **Vistas de Inventario:** Tablas dinámicas con filtros por Tipo y Estado.
* **Interfaz de Gestión:** Formularios para actualización rápida de estados por lotes.

---

## 5. Criterios de Aceptación
1.  El dashboard debe actualizarse automáticamente tras cambios de estado.
2.  Filtrado funcional por tipo de producto en toda la plataforma.
3.  **Restricción de disponibilidad:** Los ítems en estado "Roto" o "Alquilado" no pueden asignarse a nuevos eventos.

## Estructura del Repositorio
```text
├── /backend    # Node.js/TS API
├── /frontend   # Vite/Angular UI
├── docker-compose.yml
└── README.md
```

### Requisitos Previos
* Node.js (v18+)
* TypeScript
* PostgreSQL
* pnpm (Gestor de paquetes recomendado)
* Docker desktop
* Docker compose
