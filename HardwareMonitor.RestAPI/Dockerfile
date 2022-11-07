#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HardwareMonitor.RestAPI/HardwareMonitor.RestAPI.csproj", "HardwareMonitor.RestAPI/"]
RUN dotnet restore "HardwareMonitor.RestAPI/HardwareMonitor.RestAPI.csproj"
COPY . .
WORKDIR "/src/HardwareMonitor.RestAPI"
RUN dotnet build "HardwareMonitor.RestAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HardwareMonitor.RestAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HardwareMonitor.RestAPI.dll"]