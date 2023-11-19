#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Order/Order.API/Order.API.csproj", "src/Services/Order/Order.API/"]
COPY ["ECommers.Dapr/ECommers.Dapr.csproj", "ECommers.Dapr/"]
COPY ["src/Services/Order/Order.Abstraction/Order.Abstraction.csproj", "src/Services/Order/Order.Abstraction/"]
COPY ["src/Services/Order/Order.Application/Order.Application.csproj", "src/Services/Order/Order.Application/"]
COPY ["src/Framework/ECommers.Abstraction/ECommers.Abstraction.csproj", "src/Framework/ECommers.Abstraction/"]
COPY ["src/Services/Order/Order.Domain/Order.Domain.csproj", "src/Services/Order/Order.Domain/"]
COPY ["src/Framework/ECommers.Data/ECommers.Data.csproj", "src/Framework/ECommers.Data/"]
COPY ["src/Services/Order/Order.Infrastructure/Order.Infrastructure.csproj", "src/Services/Order/Order.Infrastructure/"]
RUN dotnet restore "./src/Services/Order/Order.API/./Order.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Order/Order.API"
RUN dotnet build "./Order.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Order.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.API.dll"]