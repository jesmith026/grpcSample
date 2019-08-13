FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/server/grpcSample.server.csproj", "server/"]
COPY ["src/contract/grpcSample.contract.csproj", "contract/"]
RUN dotnet restore "server/grpcSample.server.csproj"
COPY src/ .
WORKDIR "/src/server"
RUN dotnet build "grpcSample.server.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "grpcSample.server.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "grpcSample.server.dll"]
