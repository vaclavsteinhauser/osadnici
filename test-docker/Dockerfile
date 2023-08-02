FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["test-docker/test-docker.csproj", "test-docker/"]
RUN dotnet restore "test-docker/test-docker.csproj"
COPY . .
WORKDIR "/src/test-docker"
RUN dotnet build "test-docker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "test-docker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "test-docker.dll"]
