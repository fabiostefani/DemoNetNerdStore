#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./Services/Carrinho/Carrinho.API/Carrinho.API.csproj", "services/Carrinho.API/"]
COPY ["./Building blocks/Api.Core/Api.Core.csproj", "Building blocks/Api.Core/"]
COPY ["./Building blocks/Core/Core.csproj", "Building blocks/Core/"]
COPY ["./Building blocks/MessageBus/MessageBus.csproj", "Building blocks/MessageBus/"]
RUN dotnet restore "services/Carrinho.API/Carrinho.API.csproj"
COPY . .
WORKDIR "services/Carrinho.API"
RUN dotnet build "Carrinho.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Carrinho.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Carrinho.API.dll"]