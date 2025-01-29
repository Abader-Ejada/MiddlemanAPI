# Use official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project files and restore dependencies
COPY *.sln ./
COPY MiddlemanAPI/*.csproj MiddlemanAPI/
RUN dotnet restore MiddlemanAPI/MiddlemanAPI.csproj

# Copy everything and build the app
COPY MiddlemanAPI/. MiddlemanAPI/
WORKDIR /app/MiddlemanAPI
RUN dotnet publish -c Release -o /out

# Use runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .
EXPOSE 80
ENTRYPOINT ["dotnet", "MiddlemanAPI.dll"]
