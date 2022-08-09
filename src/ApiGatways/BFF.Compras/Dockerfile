FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./ApiGatways/BFF.Compras/BFF.Compras.csproj", "ApiGatways/BFF.Compras/"]
COPY ["./Building blocks/Core/Core.csproj", "Building blocks/Core/"]
COPY ["./Building blocks/Api.Core/Api.Core.csproj", "Building blocks/Api.Core/"]
COPY ["./Building blocks/MessageBus/MessageBus.csproj", "Building blocks/MessageBus/"]

RUN dotnet restore "ApiGatways/BFF.Compras/BFF.Compras.csproj"
COPY . .
WORKDIR "ApiGatways/BFF.Compras"
RUN dotnet build "BFF.Compras.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BFF.Compras.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BFF.Compras.dll"]
