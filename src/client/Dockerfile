FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/client/grpcSample.client.csproj", "client/"]
COPY ["src/contract/grpcSample.contract.csproj", "contract/"]
RUN dotnet restore "client/grpcSample.client.csproj"
COPY src/ .
WORKDIR "/src/client"
RUN dotnet build "grpcSample.client.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "grpcSample.client.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "grpcSample.client.dll"]