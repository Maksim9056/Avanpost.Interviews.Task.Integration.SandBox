# ����������� �������� ������
FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

# ������������� PostgreSQL � ������� ���������� ��� ������������
RUN apt-get update && \
    apt-get install -y postgresql postgresql-contrib && \
    mkdir -p /etc/postgresql/15/main/ && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# ��������� ������������ ��� PostgreSQL
RUN echo "host all all 0.0.0.0/0 md5" >> /etc/postgresql/15/main/pg_hba.conf && \
    echo "listen_addresses = '*'" >> /etc/postgresql/15/main/postgresql.conf && \
    echo "port = 5432" >> /etc/postgresql/15/main/postgresql.conf


# ��������� ������������ ��� PostgreSQL
RUN echo "host all all 0.0.0.0/0 md5" >> /etc/postgresql/15/main/pg_hba.conf && \
    echo "listen_addresses = '*'" >> /etc/postgresql/15/main/postgresql.conf && \
    echo "port = 5432" >> /etc/postgresql/15/main/postgresql.conf

# ����������� ������ ��� ������, ���������� � ���������� ������ �������� ��� ���������
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
# ������ PostgreSQL � ���������� ����������
CMD service postgresql start && dotnet Avanpost.Interviews.Task.Integration.SandBox.Tests.dll