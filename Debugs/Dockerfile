# ------------------
# Build Stage
# ------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY MYPERSONLA.csproj ./
RUN dotnet restore --no-cache

# Copy all other files
COPY . ./

# Publish project
RUN dotnet publish MYPERSONLA.csproj -c Release -o /app/publish

# ------------------
# Runtime Stage
# ------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish ./

EXPOSE 8080
ENTRYPOINT ["dotnet", "MYPERSONLA.dll"]
