FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./ApiGatways/BFF.Compras/BFF.Compras.csproj", "BFF.Compras/"]

RUN dotnet restore "./ApiGatways/BFF.Compras/BFF.Compras.csproj"
COPY . .
WORKDIR "BFF.Compras"
RUN dotnet build "BFF.Compras.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BFF.Compras.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BFF.Compras.dll"]
