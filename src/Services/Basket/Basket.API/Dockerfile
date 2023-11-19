#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Microservices/Services/Basket/Basket.API/Basket.API.csproj", "src/Microservices/Services/Basket/Basket.API/"]
COPY ["src/Framework/.ECP.Framework.Api/.ECP.Framework.Api.csproj", "src/Framework/.ECP.Framework.Api/"]
COPY ["src/Framework/.ECP.Framework.Abstractions/.ECP.Framework.Abstractions.csproj", "src/Framework/.ECP.Framework.Abstractions/"]
COPY ["src/Framework/.ECP.Framework.Auth/.ECP.Framework.Auth.csproj", "src/Framework/.ECP.Framework.Auth/"]
COPY ["src/Framework/.ECP.Framework.IoC/.ECP.Framework.IoC.csproj", "src/Framework/.ECP.Framework.IoC/"]
COPY ["src/Framework/.ECP.Framework.Client/.ECP.Framework.Client.csproj", "src/Framework/.ECP.Framework.Client/"]
COPY ["src/Framework/ECommers.Dapr/ECommers.Dapr.csproj", "src/Framework/ECommers.Dapr/"]
COPY ["src/Framework/.ECP.Framework.DataAudit.Common/.ECP.Framework.DataAudit.Common.csproj", "src/Framework/.ECP.Framework.DataAudit.Common/"]
COPY ["src/Framework/.ECP.Framework.DataAudit.Elasticsearch/.ECP.Framework.DataAudit.Elasticsearch.csproj", "src/Framework/.ECP.Framework.DataAudit.Elasticsearch/"]
COPY ["src/Framework/.ECP.Framework.DataAudit.RabbitMq/.ECP.Framework.DataAudit.RabbitMq.csproj", "src/Framework/.ECP.Framework.DataAudit.RabbitMq/"]
COPY ["src/Framework/.ECP.Framework.Secret/.ECP.Framework.Secret.csproj", "src/Framework/.ECP.Framework.Secret/"]
COPY ["src/Framework/.ECP.Framework.Data/.ECP.Framework.Data.csproj", "src/Framework/.ECP.Framework.Data/"]
COPY ["src/Framework/.ECP.Framework.Localization/.ECP.Framework.Localization.csproj", "src/Framework/.ECP.Framework.Localization/"]
COPY ["src/Framework/.ECP.Framework.Logging.Serilog/.ECP.Framework.Logging.Serilog.csproj", "src/Framework/.ECP.Framework.Logging.Serilog/"]
COPY ["src/Framework/.ECP.Framework.Logging/.ECP.Framework.Logging.csproj", "src/Framework/.ECP.Framework.Logging/"]
COPY ["src/Framework/.ECP.Framework.Monitoring/.ECP.Framework.Monitoring.csproj", "src/Framework/.ECP.Framework.Monitoring/"]
COPY ["src/Framework/.ECP.Framework.TaskScheduling.Hangfire.Autofac/.ECP.Framework.TaskScheduling.Hangfire.Autofac.csproj", "src/Framework/.ECP.Framework.TaskScheduling.Hangfire.Autofac/"]
COPY ["src/Framework/.ECP.Framework.TaskScheduling.Hangfire/.ECP.Framework.TaskScheduling.Hangfire.csproj", "src/Framework/.ECP.Framework.TaskScheduling.Hangfire/"]
COPY ["src/Framework/.ECP.Framework.Validation/.ECP.Framework.Validation.csproj", "src/Framework/.ECP.Framework.Validation/"]
COPY ["src/Microservices/Services/Basket/Basket.Application/Basket.Application.csproj", "src/Microservices/Services/Basket/Basket.Application/"]
COPY ["src/Microservices/Services/Basket/Basket.Abstraction/Basket.Abstraction.csproj", "src/Microservices/Services/Basket/Basket.Abstraction/"]
RUN dotnet restore "src/Microservices/Services/Basket/Basket.API/Basket.API.csproj"
COPY . .
WORKDIR "/src/src/Microservices/Services/Basket/Basket.API"
RUN dotnet build "Basket.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Basket.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.API.dll"]