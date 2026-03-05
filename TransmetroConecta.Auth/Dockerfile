FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["TransmetroConecta.Auth.API/TransmetroConecta.Auth.API.csproj", "TransmetroConecta.Auth.API/"]
COPY ["TransmetroConecta.Auth.Application/TransmetroConecta.Auth.Application.csproj", "TransmetroConecta.Auth.Application/"]
COPY ["TransmetroConecta.Auth.Domain/TransmetroConecta.Auth.Domain.csproj", "TransmetroConecta.Auth.Domain/"]
COPY ["TransmetroConecta.Auth.Infrastructure/TransmetroConecta.Auth.Infrastructure.csproj", "TransmetroConecta.Auth.Infrastructure/"]

RUN dotnet restore "TransmetroConecta.Auth.API/TransmetroConecta.Auth.API.csproj"

COPY . .
WORKDIR "/src/TransmetroConecta.Auth.API"
RUN dotnet build "TransmetroConecta.Auth.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TransmetroConecta.Auth.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TransmetroConecta.Auth.API.dll"]