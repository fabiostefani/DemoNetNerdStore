#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./Services/Pedido/Pedidos.API/Pedidos.API.csproj", "services/Pedido/Pedidos.API/"]
COPY ["./Building blocks/Api.Core/Api.Core.csproj", "Building blocks/Api.Core/"]
COPY ["./Building blocks/Core/Core.csproj", "Building blocks/Core/"]
COPY ["./Building blocks/MessageBus/MessageBus.csproj", "Building blocks/MessageBus/"]
COPY ["./Services/Pedido/Pedidos.Infra/Pedidos.Infra.csproj", "Services/Pedido/Pedidos.Infra/"]
COPY ["./Services/Pedido/Pedidos.Domain/Pedidos.Domain.csproj", "Services/Pedido/Pedidos.Domain/"]
RUN dotnet restore "src/services/Pedido/Pedidos.API/Pedidos.API.csproj"
COPY . .
WORKDIR "services/Pedido/Pedidos.API"
RUN dotnet build "Pedidos.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pedidos.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pedidos.API.dll"]