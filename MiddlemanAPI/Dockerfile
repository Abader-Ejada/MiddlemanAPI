# Use the official .NET SDK image for .NET 8
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy the csproj and restore any dependencies (via dotnet restore)
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the code
COPY . ./

# Publish the app
RUN dotnet publish -c Release -o /app/publish

# Define entrypoint
ENTRYPOINT ["dotnet", "/app/publish/MiddlemanAPI.dll"]
