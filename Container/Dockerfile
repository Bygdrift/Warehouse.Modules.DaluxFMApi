#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Container.csproj", ""]
RUN dotnet restore "Container.csproj"
COPY . .
WORKDIR "/src/Container"
RUN dotnet build "Container.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Container.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Container.dll"]