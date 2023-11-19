#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Product/Product.API/Product.API.csproj", "src/Services/Product/Product.API/"]
COPY ["ECommers.Dapr/ECommers.Dapr.csproj", "ECommers.Dapr/"]
COPY ["src/Services/Product/Product.Application/Product.Application.csproj", "src/Services/Product/Product.Application/"]
COPY ["src/Framework/ECommers.Abstraction/ECommers.Abstraction.csproj", "src/Framework/ECommers.Abstraction/"]
COPY ["src/Services/Product/Product.Abstraction/Product.Abstraction.csproj", "src/Services/Product/Product.Abstraction/"]
COPY ["src/Services/Product/Product.Domain/Product.Domain.csproj", "src/Services/Product/Product.Domain/"]
COPY ["src/Framework/ECommers.Data/ECommers.Data.csproj", "src/Framework/ECommers.Data/"]
COPY ["src/Services/Product/Product.Infrastructure/Product.Infrastructure.csproj", "src/Services/Product/Product.Infrastructure/"]
RUN dotnet restore "./src/Services/Product/Product.API/./Product.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Product/Product.API"
RUN dotnet build "./Product.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Product.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Product.API.dll"]