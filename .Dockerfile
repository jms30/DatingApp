# must run from root of the dating app folder
FROM mcr.microsoft.com/dotnet/aspnet:5.0 as base
WORKDIR /app

# restore and publish your solution app.
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["API", "API"]
COPY ["DatabaseInitializer", "DatabaseInitializer"]
WORKDIR /src
RUN dotnet restore "./API/API.csproj"

# build your API image 
WORKDIR /src/API
RUN dotnet build "API.csproj" -c Release -o /app/build

# publish API image after building.
FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish

# Build runtime image
FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "API.dll"]