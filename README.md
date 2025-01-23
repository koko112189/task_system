##Task Management System

Este proyecto es un sistema de gestión de tareas desarrollado con una arquitectura hexagonal (también conocida como "puertos y adaptadores"), utilizando el patrón Unit of Work para la gestión de transacciones. El backend está construido con ASP.NET Core y puede conectarse a SQL Server o PostgreSQL.


Requisitos Previos
Antes de desplegar el proyecto, asegúrate de tener instalado lo siguiente:


- .NET SDK 6.0 o superior: Descargar .NET SDK

- SQL Server o PostgreSQL: Dependiendo de la base de datos que desees utilizar.

- Para SQL Server: Descargar SQL Server

- Para PostgreSQL: Descargar PostgreSQL

- Entity Framework Core CLI: Para ejecutar migraciones.

- Instalar con: dotnet tool install --global dotnet-ef

- Docker (opcional): Si deseas desplegar el proyecto en contenedores.

# Configuración del Proyecto
1. Configuración de la Base de Datos
2. 
El proyecto puede conectarse a SQL Server o PostgreSQL. Asegúrate de configurar la cadena de conexión en el archivo appsettings.json

SQL Server



"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=TaskSystemDB;User Id=sa;Password=test1234;Trust Server Certificate=true;"
}

PostgreSQL



"ConnectionStrings": {
  "PostgresConnection": "Host=localhost;Database=TaskSystemDB;Username=postgres;Password=your_password"
}

2. Configuración del Proyecto
3. 
Clona el repositorio:

bash

git clone https://github.com/koko112189/task_system.git

cd task-system

Configura la base de datos:

Abre el archivo appsettings.json y modifica las cadenas de conexión según tu entorno.


Instala las dependencias:


bash


dotnet restore

##Aplica las migraciones:


- Para SQL Server:

bash

dotnet ef database update --context TasksDbContext


Para PostgreSQL:

bash

dotnet ef database update --context TasksDbContext

3. Ejecución del Proyecto
Ejecuta el proyecto:

bash

dotnet run

Accede a la aplicación:

Abre tu navegador y visita https://localhost:5001 (o http://localhost:5000 si no usas HTTPS).

Swagger UI:


Para explorar la API, visita https://localhost:5001/swagger.

Despliegue en Docker (Opcional)
Si deseas desplegar el proyecto en Docker, sigue estos pasos:

Crea un archivo Dockerfile:

dockerfile

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
 . .
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
 --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TasksWeb.dll"]
Crea un archivo docker-compose.yml:

yaml

version: '3.8'
services:
  web:
    image: task-management-system
    build:
      context: .
      dockerfile: Dockerfile
    ports:
      - "5000:80"
      - "5001:443"
    environment:
      - ConnectionStrings__DefaultConnection=Server=db;Database=TaskSystemDB;User Id=sa;Password=test1234;Trust Server Certificate=true;
    depends_on:
      - db

  db:
    image: mcr.microsoft.com/mssql/server:2019-latest
    environment:
      SA_PASSWORD: "test1234"
      ACCEPT_EULA: "Y"
    ports:
      - "1433:1433"
Construye y ejecuta los contenedores:

bash

docker-compose up --build
Accede a la aplicación:

Abre tu navegador y visita http://localhost:5000.

## **Estructura del Proyecto**

El proyecto sigue una arquitectura hexagonal con las siguientes capas:

Application: Contiene la lógica de negocio, DTOs, servicios y mapeos.

Domain: Contiene las entidades y interfaces de repositorio.

Infrastructure: Implementa los repositorios, el contexto de la base de datos y la configuración de Entity Framework.

WebAPI: Contiene los controladores, middleware y configuración de la API.

Pruebas Unitarias
El proyecto incluye pruebas unitarias para los controladores y servicios. Para ejecutar las pruebas:

bash

dotnet test
