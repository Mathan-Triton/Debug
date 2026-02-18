# ------------------
# Build Stage
# ------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY MYPERSONLA/MYPERSONLA.csproj ./ 
RUN dotnet restore

# Copy all project files
COPY MYPERSONLA/ ./ 
RUN dotnet publish -c Release -o /app/publish --no-restore

# ------------------
# Runtime Stage
# ------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Set ASP.NET Core to listen on port 8080
ENV ASPNETCORE_URLS=http://+:8080

# Copy published files from build stage
COPY --from=build /app/publish ./

EXPOSE 8080

ENTRYPOINT ["dotnet", "MYPERSONLA.dll"]
