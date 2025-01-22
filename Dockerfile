# Usar la imagen base de .NET 9 SDK para compilar la aplicaci�n
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar los archivos de proyecto y restaurar las dependencias
COPY *.sln .
COPY src/TasksWeb/*.csproj ./src/TasksWeb/
COPY src/Application/*.csproj ./src/Application/
COPY src/Domain/*.csproj ./src/Domain/
COPY src/Infrastructure/*.csproj ./src/Infrastructure/
RUN dotnet restore

# Copiar el resto de los archivos y compilar la aplicaci�n
COPY . .
WORKDIR /app/src/TasksWeb
RUN dotnet publish -c Release -o /out

# Usar la imagen base de .NET 9 Runtime para ejecutar la aplicaci�n
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /out .

# Exponer el puerto en el que la aplicaci�n escuchar�
EXPOSE 80

# Configurar la entrada del contenedor para ejecutar la aplicaci�n
ENTRYPOINT ["dotnet", "TasksWeb.dll"]