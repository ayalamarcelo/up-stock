# Guía de Git: Flujo de Trabajo y Comandos Esenciales

## 1. Establece un vínculo con el repositorio original(Solo una vez, si ya lo hiciste saltá al próximo paso)
Crea un nuevo "remoto" llamado upstream que apunta al repositorio de donde sacaste tu copia. Por defecto, tu repositorio ya tiene uno llamado origin (que es tu propia copia en GitHub). Con esto, ahora tu Git conoce dos fuentes: la tuya y la original.

`git remote add upstream https://github.com/ayalamarcelo/pp2-proj-ifts11`

## 2. Descarga la información más reciente del autor original.
Trae todos los cambios, ramas y etiquetas del repositorio original a tu computadora, pero sin modificar tu código todavía. Es como decir: "Git, ve a ver qué hay de nuevo allá afuera y guárdalo en una carpeta temporal para que yo lo revise".

`git fetch upstream`

## 3. Compara lo que tú tienes con lo que hay en el original.
Muestra una lista resumida (en una sola línea por cada cambio) de todos los commits que existen en el repositorio original (upstream/main) pero que tú todavía no tienes en tu rama local (main). Te sirve para saber exactamente qué vas a traer antes de hacerlo.

`git log main..upstream/main --oneline`

## 4. Combina los cambios del original en tu proyecto local.
Toma todas las novedades que descargaste con el fetch y las mezcla con tu código actual. Si no hay conflictos, tu versión local quedará exactamente igual a la del autor original.

`git merge upstream/main`

## 5. Sube los cambios actualizados a tu cuenta de GitHub.
Ahora que tu computadora tiene el código al día, este comando envía esos cambios de tu máquina a tu propio repositorio remoto (origin). Así, tu perfil de GitHub también estará sincronizado con el proyecto original.

`git push origin main`


---



# Crear Ramas

## 1. Crear una rama para tus cambios

Antes de hacer nada, crea una rama nueva. Así mantienes tu main limpio.

`git checkout -b <nombre-de-rama>`
(Esto crea la rama y te mueve a ella automáticamente).

## 2. Hacer cambios y subirlos (Push)

Una vez que hayas editado tus archivos:

```bash
git add .  // (Preparas los cambios).

git commit -m "Descripción de lo que hice" // (Le pones nombre al cambio).

git push origin mi-rama
```
¿Qué hace esto? Sube tu rama con tus cambios a tu repositorio en GitHub.


## 3. Crear el Pull Request (PR)

El PR no se hace por comandos de consola, sino en la interfaz web de GitHub.

- Entra a tu repositorio en GitHub.

- Verás un cartel amarillo que dice "Compare & pull request". Haz clic ahí.

- Asegúrate de que la flecha indique: base: main ← compare: mi-nueva-funcionalidad.

- Escribe un título, una descripción y dale al botón "Create pull request".


## 4. Mergear (Combinar) el PR

Una vez que el PR está creado, alguien (o tú mismo) debe revisarlo. Si todo está bien:

- En la misma página del PR en GitHub, baja hasta el botón verde que dice "Merge pull request".

- Confirma el merge.

¿Qué hace esto? GitHub toma los cambios de tu rama y los mete oficialmente en la rama main del proyecto.


## 5. Limpiar y actualizar localmente

Ahora que el código está mergeado en GitHub, tu computadora "no lo sabe" todavía. Tienes que traer esos cambios de vuelta:

`git checkout main (Vuelves a tu rama principal).`

`git pull upstream main (Descargas el merge que acabas de hacer en github).`

`git branch -d mi-rama (Borras la rama local, ya que ya no la necesitas).`


# Deshacer o corregir commits
Si el commit es el último que hiciste y solo quieres corregir el texto o agregar un archivo que olvidaste:

Comando: `git commit --amend -m "Nuevo mensaje corregido"`
Qué hace: Abre el último commit y lo reemplaza por uno nuevo con las correcciones.


A. Soft Reset (El más seguro)

Comando: `git reset --soft HEAD~1`

Qué hace: Borra el último commit, pero mantiene tus cambios intactos en el área de preparación (staging). Es como si nunca hubieras hecho el commit, pero el código modificado sigue ahí listo para volver a ser commiteado.

B. Mixed Reset (El por defecto)

Comando: `git reset HEAD~1`

Qué hace: Borra el commit y saca los cambios del área de preparación, pero los mantiene en tu carpeta. Los archivos aparecerán como "modificados" pero no listos para commit.

C. Hard Reset (El peligroso)

Comando: `git reset --hard HEAD~1`

Qué hace: Borra todo. Elimina el commit y descarta todos los cambios que hiciste en los archivos. No hay vuelta atrás fácil, así que úsalo solo si quieres tirar el trabajo a la basura.

Nota: El HEAD~1 significa "un commit atrás". Si quieres volver 3 commits, usarías HEAD~3.


## "Revert": Deshacer sin borrar la historia

Si ya subiste (hiciste push) tus cambios a GitHub y estás trabajando con otras personas, no uses reset. Usa revert.

Comando: `git revert [ID_DEL_COMMIT]` => cómo saber el id del commit => `git log` => `git log --online` te dá el id resumido

Qué hace: Crea un nuevo commit que hace exactamente lo contrario al commit que quieres deshacer. Es la forma limpia y profesional de arreglar un error en proyectos compartidos porque no borras el historial, sino que añades una corrección.

## Eliminar una rama

`git branch -d nombre-de-la-rama`


> [!NOTE] amend para mensajes, reset para local, revert para lo que ya subiste a la nube.


---

# Correr el proyecto

1. si ya forkeamos y clonamos el repo a nuestro escritorio:
2. El proyecto corre con pnpm, asegurarnos de tener la herramienta.
3. 

```bash
cd backend // entrar al directorio backend
pnpm install // bajamos dependencias
pnpm run dev // corremos el servidor
```