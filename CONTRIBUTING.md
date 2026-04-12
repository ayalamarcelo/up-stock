# 🛠 Guía de Contribución y Flujo de Trabajo (Git Flow)

Esta guía explica cómo manejamos las ramas en el proyecto **Stockly** para mantener un desarrollo organizado, seguro y escalable.

## 🌳 Estructura de Ramas

Utilizamos una adaptación del modelo **Git Flow**, dividiendo el trabajo en tres niveles de importancia:

### 1. `main` (Producción)
* **Propósito:** Contiene el código estable y listo para el usuario final.
* **Regla de Oro:** Nunca se trabaja directamente sobre esta rama.
* **Despliegue:** Todo lo que esté en `main` debe ser código que funciona al 100%.

### 2. `dev` (Desarrollo / Integración)
* **Propósito:** Es la rama principal de trabajo. Aquí se combinan todas las nuevas funcionalidades antes de pasar a producción.
* **Uso:** Sirve como base para crear nuevas ramas `feature`.
* **Estabilidad:** Debe ser funcional, pero puede contener errores menores que se están puliendo.

### 3. `feature/` (Funcionalidades)
* **Propósito:** Se utiliza para desarrollar tareas específicas (ej: crear un modelo, un endpoint o un componente).
* **Nombre estándar:** `feature/nombre-de-la-tarea` o `feat/nombre-de-la-tarea`.
* **Ciclo de vida:** Se crea a partir de `dev` y se elimina después de haberse fusionado con éxito.

---

## 🔄 Flujo de Trabajo Diario

Para agregar una nueva funcionalidad (como el modelo de activos), sigue estos pasos:

### Paso 1: Crear la rama de funcionalidad
Asegúrate de estar en `dev` y tener lo último del servidor antes de empezar:
```bash
git checkout dev
git pull origin dev
git checkout -b feature/nombre-de-tu-tarea
```

### Paso 2: Trabajar y guardar cambios
Realiza tus cambios en el código y crea commits locales. Asegúrate de que cada commit represente una unidad lógica de trabajo:

```bash
git add .
git commit -m "tipo: descripción corta de lo que hiciste"
```

### Paso 3: Subir cambios a tu rama en GitHub
Envía tus avances a la nube para respaldo o para que otros puedan revisar tu código:

```bash
git push origin feature/nombre-de-tu-tarea
```

### Paso 4: Fusionar con dev

Una vez que la tarea esté terminada, probada y los modelos/endpoints funcionen correctamente, integra los cambios en la rama de desarrollo:

```bash
git checkout dev
git merge feature/nombre-de-tu-tarea
git push origin dev
```

## 📝 Convenciones de Commits

Utilizamos prefijos para que el historial del proyecto sea legible y fácil de auditar. El formato es: tipo: descripción.

Prefijo	Descripción	Ejemplo:

```bash
feat: Nuevas características o funcionalidades.	
feat: add asset model
fix: Corrección de errores o fallos técnicos.
fix: repair stock calculation
docs: Cambios exclusivos en la documentación.	docs: update contributing guide
refactor: Cambios en el código que no alteran la lógica.	
refactor: organize folders
chore:	Tareas de mantenimiento o configuración (ej: instalar tipos).
chore: install @types/express
```
