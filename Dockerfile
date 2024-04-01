#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EAgendaMedica.WebApi/EAgendaMedica.WebApi.csproj", "EAgendaMedica.WebApi/"]
COPY ["EAgendaMedica.Aplicacao/EAgendaMedica.Aplicacao.csproj", "EAgendaMedica.Aplicacao/"]
COPY ["EAgendaMedica.Dominio/EAgendaMedica.Dominio.csproj", "EAgendaMedica.Dominio/"]
COPY ["EAgendaMedica.Infra/EAgendaMedica.Infra.csproj", "EAgendaMedica.Infra/"]


RUN dotnet restore "EAgendaMedica.WebApi/EAgendaMedica.WebApi.csproj"
COPY . .
WORKDIR "/src/EAgendaMedica.WebApi"
RUN dotnet build "EAgendaMedica.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EAgendaMedica.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EAgendaMedica.WebApi.dll"]