# OnOff -- Prueba Técnica Semi Senior .NET

Autor: Ingeniero Pablo Miguez

Aplicación To-Do Full-Stack con .NET 9, Angular 17+, JWT,
NgRx, Material Design y Pruebas Automatizadas.

Este proyecto fue desarrollado como parte de la Etapa 4 -- Prueba
técnica virtual del proceso de selección para el cargo Desarrollador
Semi Senior .NET en OnOff.

La solución incluye:

-   API REST robusta, modular y segura con JWT.\
-   Frontend moderno con Angular 17+ y Angular Material.\
-   Manejo profesional del estado con NgRx.\
-   Autenticación completa en frontend y backend.\
-   Arquitectura limpia, separación de capas, principios SOLID.\
-   Pruebas automáticas en backend (xUnit) y frontend (Jasmine/Karma).\
-   Diseño responsive con toolbar, sidebar y layout tipo aplicación
    profesional.\
-   Documentación clara para ejecución, instalación, decisiones técnicas
    y flujo funcional.

------------------------------------------------------------------------

# 1.  Objetivo del Proyecto

Construir una aplicación "To-Do" con autenticación y manejo de tareas:

-   Login de usuario.
-   Gestión de tareas (listar, crear, editar, eliminar, marcar como
    completadas).
-   Dashboard con métricas (total, completadas, pendientes).
-   Persistencia en base de datos.
-   Backend en .NET 9 con JWT.
-   Frontend moderno y modular con Angular 17+.

------------------------------------------------------------------------

# 2.  Arquitectura General

La solución está dividida en dos proyectos:

    ONOFF-PRUEBA-SEMISENIOR/
    │── onoff-todo-web/         → Frontend Angular 17+
    │── OnOff.Todo.Api/         → Backend .NET 9 API
    │── OnOff.Todo.Api.Tests/   → Pruebas automatizadas backend (xUnit)
    │── README.md
    │── OnOff-Prueba-SemiSenior.sln
    │── .gitignore

------------------------------------------------------------------------

# 3.  Backend (.NET 9 API)

## 3.1 Tecnologías utilizadas

-   .NET 9 Web API\
-   Entity Framework Core 9\
-   JWT Authentication\
-   ASP.NET Authorization\
-   Dependency Injection\
-   xUnit y Moq para pruebas

------------------------------------------------------------------------

## 3.2 Arquitectura Backend

    OnOff.Todo.Api/
    │── Controllers/
    │── Application/
    │── Domain/
    │── Infrastructure/
    │── Program.cs
    │── appsettings.json

### ✔ Domain

Entidades como `ApplicationUser` y `TodoTask`.

### ✔ Infrastructure

`ApplicationDbContext`, SQL Server o InMemory, repositorios EF Core.

### ✔ Application

Servicios, DTOs y lógica de negocio.

### ✔ API

Controladores expuestos como REST endpoints.

------------------------------------------------------------------------

## 3.3 Endpoints principales

###  Autenticación

POST `/api/Auth/login`

###  Tareas

GET `/api/TodoTasks?filter=all|pending|completed`\
POST `/api/TodoTasks`\
PUT `/api/TodoTasks/{id}`\
DELETE `/api/TodoTasks/{id}`

###  Dashboard

GET `/api/TodoTasks/dashboard`

------------------------------------------------------------------------

## 3.4 Autenticación con JWT

El backend genera un token JWT con:

-   ID del usuario (`sub`)
-   Email
-   Expiración
-   Firma con clave simétrica

El frontend lo envía en:

    Authorization: Bearer <token>

------------------------------------------------------------------------

# 4.  Frontend (Angular 17+)

## 4.1 Tecnologías utilizadas

-   Angular 17+\
-   Angular Material\
-   NgRx (Store, Effects, Reducers, Selectors)\
-   Reactive Forms\
-   RxJS\
-   Jasmine/Karma

------------------------------------------------------------------------

## 4.2 Arquitectura Frontend

    src/app/
    │── core/
    │── features/
    │   │── auth/
    │   │── dashboard/
    │   │── todo/
    │── shared/
    │── app.component.ts
    │── app-routing.module.ts

###  Core: servicios, guards e interceptores

###  Features: módulos de negocio (auth, todo, dashboard)

###  Shared: componentes comunes

------------------------------------------------------------------------

## 4.3 Seguridad en frontend

-   **AuthGuard:** protege rutas privadas.\
-   **AuthInterceptor:** agrega el token JWT a cada solicitud.

------------------------------------------------------------------------

## 4.4 UI y Responsiveness

-   Sidenav + Toolbar con Angular Material.\
-   Layout flexible y escalable.

------------------------------------------------------------------------

## 4.5 Optimización: trackBy

Implementación en `ListComponent` para mejorar rendimiento:

``` ts
trackByTaskId(index: number, task: TodoTask) {
  return task.id;
}
```

------------------------------------------------------------------------

# 5.  Pruebas Automatizadas

## 5.1 Backend -- xUnit

Ejecutar:

``` bash
dotnet test
```

Pruebas incluidas:

-   Filtrado de tareas\
-   Creación de tareas\
-   Controladores con Moq

------------------------------------------------------------------------

## 5.2 Frontend -- Jasmine/Karma

Ejecutar:

``` bash
npm test
```

Pruebas importantes:

-   `LoginComponent`
    -   formulario inválido (vacío)
    -   formulario válido (despacha login)
-   `AppComponent`
    -   renderización correcta del layout

------------------------------------------------------------------------

# 6.  Cómo Ejecutar el Proyecto

## 6.1 Backend

``` bash
cd OnOff.Todo.Api
dotnet restore
dotnet run
```

### URLs

    https://localhost:5202
    
------------------------------------------------------------------------

## 6.2 Frontend

``` bash
cd onoff-todo-web
npm install
ng serve
```

Abrir:

    http://localhost:4200

------------------------------------------------------------------------

# 7. Credenciales de Prueba

    email: demo@onoff.com
    password: 123456

------------------------------------------------------------------------

# 8. Decisiones Técnicas

-   No se utilizó Identity completo para evitar sobrecarga.\
-   Uso de JWT manual siguiendo estándares modernos.\
-   NgRx para estado global escalable.\
-   Arquitectura modular y limpia.\
-   Interceptor + Guards para seguridad.

------------------------------------------------------------------------

# 9.  Buenas Prácticas Aplicadas

-   SOLID\
-   Arquitectura limpia\
-   Modularización\
-   DTOs\
-   Comentarios claros\
-   trackBy en listas\
-   Responsiveness\
-   Efectos de NgRx para side-effects limpios

------------------------------------------------------------------------

# 10.  Conclusión

El proyecto implementa de manera completa y profesional todos los
requisitos solicitados por OnOff:

-   Backend moderno con .NET 9 y JWT\
-   Frontend escalable con Angular y NgRx\
-   Pruebas automáticas en ambas capas\
-   Arquitectura limpia y mantenible\
-   Documentación clara y profesional

**Autor:**\
\ Ingeniero Pablo Miguez
