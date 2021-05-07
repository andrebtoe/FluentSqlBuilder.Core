#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["FluentSqlBuilder.Playground/FluentSqlBuilder.Playground.csproj", "FluentSqlBuilder.Playground/"]
COPY ["FluentSqlBuilder.Core/FluentSqlBuilder.Core.csproj", "FluentSqlBuilder.Core/"]
COPY ["FluentSqlBuilder.Data/FluentSqlBuilder.Data.csproj", "FluentSqlBuilder.Data/"]
RUN dotnet restore "FluentSqlBuilder.Playground/FluentSqlBuilder.Playground.csproj"
COPY . .
WORKDIR "/src/FluentSqlBuilder.Playground"
RUN dotnet build "FluentSqlBuilder.Playground.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FluentSqlBuilder.Playground.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FluentSqlBuilder.Playground.dll"]