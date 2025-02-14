# #See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UnpakCbt/UnpakCbt.Api.csproj", "UnpakCbt/"]
RUN dotnet restore "./UnpakCbt/UnpakCbt.Api.csproj"
COPY . .
WORKDIR "/src/UnpakCbt"
RUN dotnet build "./UnpakCbt.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UnpakCbt.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Change ownership of the application files to the non-root user
RUN chown -R app:app /app

# Set strict permissions for files and directories
RUN find /app -type f -exec chmod 400 {} \;
RUN find /app -type d -exec chmod 600 {} \;

# Switch to the non-root user
USER app

ENTRYPOINT ["dotnet", "UnpakCbt.Api.dll"]



# # Base image to run the application
# FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base

# # Install necessary dependencies (Alpine uses apk instead of apt-get)
# RUN apk add --no-cache shadow

# # Create a non-root user and group to run the app
# RUN addgroup -S appuser && adduser -S appuser -G appuser

# # Set the working directory
# WORKDIR /app

# # Expose the ports the application will use
# EXPOSE 8080
# EXPOSE 8081

# # Set strict permissions for security
# RUN chmod -R 700 /app

# # Build stage using the SDK image
# FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
# ARG BUILD_CONFIGURATION=Release

# # Set the working directory in the build container
# WORKDIR /src

# # Copy the project file(s) and restore dependencies
# COPY ["UnpakCbt/UnpakCbt.Api.csproj", "UnpakCbt/"]
# RUN --mount=type=cache,target=/root/.nuget/packages dotnet restore "./UnpakCbt/UnpakCbt.Api.csproj"

# # Copy the rest of the application code
# COPY . .

# # Set the working directory to the project folder
# WORKDIR "/src/UnpakCbt"

# # Build the project
# RUN dotnet build "./UnpakCbt.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# # Publish stage to create the final build output
# FROM build AS publish
# ARG BUILD_CONFIGURATION=Release
# RUN dotnet publish "./UnpakCbt.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# # Final image, using the base image and adding the published app
# FROM base AS final

# # Set the working directory to the app folder
# WORKDIR /app

# # Copy the published app from the build stage
# COPY --from=publish /app/publish .

# # Change ownership of the application files to the non-root user
# RUN chown -R appuser:appuser /app

# # Set strict permissions for security
# RUN find /app -type f -exec chmod 600 {} \;
# RUN find /app -type d -exec chmod 700 {} \;

# # Switch to the non-root user
# USER appuser

# # Set the entry point to run the application
# ENTRYPOINT ["dotnet", "UnpakCbt.Api.dll"]
