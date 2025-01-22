# Space Management Service

Este proyecto es un servicio de gestión de espacios que permite crear, actualizar, eliminar y consultar espacios y reservas. Está construido con ASP.NET Core y utiliza Entity Framework Core para la persistencia de datos.

## Características

- Crear, actualizar, eliminar y consultar espacios.
- Crear, actualizar, eliminar y consultar reservas.
- Validación de solapamiento de reservas.
- Configuración de CORS para permitir solicitudes desde otros dominios.
- Documentación de API con Swagger.

## Requisitos

- .NET 9
- SQL Server (o cualquier base de datos compatible con Entity Framework Core)
- Visual Studio 2022 (o cualquier IDE compatible con .NET)

## Configuración del Proyecto

1. Clona el repositorio:

git clone https://github.com/koko112189/RentHub_Backend_msReservations.git
cd RentHub_Backend

2. Configura la cadena de conexión a la base de datos en `appsettings.json`:
3. Restaura los paquetes NuGet:
   dotnet restore
4. Aplica las migraciones de la base de datos:
   dotnet ef database update

## Ejecución del Proyecto

1. Ejecuta el proyecto:
   dotnet run --project src/WebAPI
   dotnet run --project src/SpacesAPI

2. Abre tu navegador y navega a `https://localhost:44365/swagger` para ver la documentación de la API de reservas.
3. Abre tu navegador y navega a `https://localhost:44354/swagger` para ver la documentación de la API de espacios.

Arquitectura Utilizada
La solución sigue una arquitectura Clean Code, que separa las responsabilidades en diferentes capas para mejorar la mantenibilidad, escalabilidad y testabilidad del sistema.

## Estructura del Proyecto

- `src/Application`: Contiene la lógica de negocio y los servicios.
- `src/Domain`: Contiene las entidades y excepciones del dominio.
- `src/Infrastructure`: Contiene la implementación de la persistencia y los repositorios.
- `src/WebAPI`: Contiene los controladores y la configuración de la API para gestionar reservas.
- `src/SpacesAPI`: Contiene los controladores y la configuración de la API para gestionar espacios.
- `tests`: Contiene las pruebas unitarias y de integración.

## Cómo Ejecutar las Pruebas
•	Las pruebas unitarias se encuentran en el proyecto tests.
•	Utilizan xUnit como framework de pruebas y Moq para crear simulaciones de dependencias.
•	Para ejecutar las pruebas unitarias, puedes usar el siguiente comando en la terminal:

 dotnet test

   
