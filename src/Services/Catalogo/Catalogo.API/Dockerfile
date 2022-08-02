#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./Services/Catalogo/Catalogo.API/Catalogo.API.csproj", "services/Catalogo/Catalogo.API/"]               
COPY ["./Building blocks/Core/Core.csproj", "Building blocks/Core/"]
COPY ["./Building blocks/Api.Core/Api.Core.csproj", "Building blocks/Api.Core/"]
COPY ["./Building blocks/MessageBus/MessageBus.csproj", "Building blocks/MessageBus/"]
RUN dotnet restore "services/Catalogo/Catalogo.API/Catalogo.API.csproj"
COPY . .
WORKDIR "services/Catalogo/Catalogo.API"
RUN dotnet build "Catalogo.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Catalogo.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalogo.API.dll"]