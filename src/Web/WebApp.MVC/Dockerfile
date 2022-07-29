#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./Web/WebApp.MVC/WebApp.MVC.csproj", "web/WebApp.MVC/"]
COPY ["./Building blocks/Api.Core/Api.Core.csproj", "building blocks/WebApi.Core/"]
COPY ["./Building blocks/Core/Core.csproj", "building blocks/WebApi.Core/"]

RUN dotnet restore "web/WebApp.MVC/WebApp.MVC.csproj"
COPY . .
WORKDIR "web/WebApp.MVC/"
RUN dotnet build "WebApp.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApp.MVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApp.MVC.dll"]

