#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BlazorServer/BlazorServer.csproj", "BlazorServer/"]
#COPY ["MCWebApplication1.ServiceDefaults/MCWebApplication1.ServiceDefaults.csproj", "MCWebApplication1.ServiceDefaults/"]
RUN dotnet restore "./BlazorServer/BlazorServer.csproj"
COPY . .
WORKDIR "/src/BlazorServer"
RUN dotnet build "./BlazorServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BlazorServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENV HIGHGEEK_APPNAME=prod:dotnet_blazorserver
### Install the agent
##USER root
##RUN apt-get update && apt-get install -y wget ca-certificates gnupg \
##&& echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
##&& wget https://download.newrelic.com/548C16BF.gpg \
##&& apt-key add 548C16BF.gpg \
##&& apt-get update \
##&& apt-get install -y 'newrelic-dotnet-agent' \
##&& rm -rf /var/lib/apt/lists/*
### Enable the agent
### RUN commands
##USER 1001
##ENV CORECLR_ENABLE_PROFILING=1 \
##CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
##CORECLR_NEWRELIC_HOME=/usr/local/newrelic-dotnet-agent \
##CORECLR_PROFILER_PATH=/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so \
##NEW_RELIC_LICENSE_KEY= \
##NEW_RELIC_APP_NAME="BlazorServer" \
##NEW_RELIC_REGION=eu

ENTRYPOINT ["dotnet", "BlazorServer.dll"]