# Étape 1 : Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copier solution et projets
COPY *.sln .
COPY DiveHub.Application/*.csproj DiveHub.Application/
COPY DiveHub.Core/*.csproj DiveHub.Core/
COPY DiveHub.Infrastructure/*.csproj DiveHub.Infrastructure/
COPY DiveHub.Tests/*.csproj DiveHub.Tests/
COPY DiveHub.WebApi/*.csproj DiveHub.WebApi/

# Restore des dépendances
RUN dotnet restore

# Copier tout le code
COPY . .

# Publier uniquement l'API
WORKDIR /src/DiveHub.WebApi
RUN dotnet publish -c Release -o /app

# Étape 2 : Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000
ENTRYPOINT ["dotnet", "DiveHub.WebApi.dll"]