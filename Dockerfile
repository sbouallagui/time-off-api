# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

# Build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src


COPY src/Time.Off.Domain/Time.Off.Domain.csproj ./Time.Off.Domain/
COPY src/Time.Off.Application/Time.Off.Application.csproj ./Time.Off.Application/
COPY src/Time.Off.Infrastructure/Time.Off.Infrastructure.csproj ./Time.Off.Infrastructure/
COPY src/Time.Off.Api/Time.Off.Api.csproj ./Time.Off.Api/

RUN dotnet restore "Time.Off.Api/Time.Off.Api.csproj"

COPY ../src/ ./ 

# Build
RUN dotnet build ./Time.Off.Api/Time.Off.Api.csproj -c Release -o /app/build --no-restore

# Publish
RUN dotnet publish ./Time.Off.Api/Time.Off.Api.csproj -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Time.Off.Api.dll"]
