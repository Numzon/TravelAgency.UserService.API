#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TravelAgency.UserService.API/TravelAgency.UserService.API.csproj", "TravelAgency.UserService.API/"]
COPY ["TravelAgency.UserService.Application/TravelAgency.UserService.Application.csproj", "TravelAgency.UserService.Application/"]
COPY ["TravelAgency.UserService.Domain/TravelAgency.UserService.Domain.csproj", "TravelAgency.UserService.Domain/"]
RUN dotnet restore "TravelAgency.UserService.API/TravelAgency.UserService.API.csproj"
COPY . .
WORKDIR "/src/TravelAgency.UserService.API"
RUN dotnet build "TravelAgency.UserService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TravelAgency.UserService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TravelAgency.UserService.API.dll"]