# Imagen base para ejecutar
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Imagen para compilar
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar proyecto y restaurar
COPY ["aguasdem-ms-plataforma-rpa.csproj", "./"]
RUN dotnet restore "aguasdem-ms-plataforma-rpa.csproj"

# Copiar el resto y compilar
COPY . .
RUN dotnet build "aguasdem-ms-plataforma-rpa.csproj" -c Release -o /app/build

# Publicar proyecto
FROM build AS publish
RUN dotnet publish "aguasdem-ms-plataforma-rpa.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "aguasdem-ms-plataforma-rpa.dll"]