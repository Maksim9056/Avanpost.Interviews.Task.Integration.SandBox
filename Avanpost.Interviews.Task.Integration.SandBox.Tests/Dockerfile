#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Avanpost.Interviews.Task.Integration.SandBox.Tests/Avanpost.Interviews.Task.Integration.SandBox.Tests.csproj", "Avanpost.Interviews.Task.Integration.SandBox.Tests/"]
COPY ["Avanpost.Interviews.Task.Integration.SandBox.Connector/Avanpost.Interviews.Task.Integration.SandBox.Connector.csproj", "Avanpost.Interviews.Task.Integration.SandBox.Connector/"]
RUN dotnet restore "./Avanpost.Interviews.Task.Integration.SandBox.Tests/./Avanpost.Interviews.Task.Integration.SandBox.Tests.csproj"
COPY . .
WORKDIR "/src/Avanpost.Interviews.Task.Integration.SandBox.Tests"
RUN dotnet build "./Avanpost.Interviews.Task.Integration.SandBox.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Avanpost.Interviews.Task.Integration.SandBox.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Avanpost.Interviews.Task.Integration.SandBox.Tests.dll"]