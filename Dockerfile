#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["AuthenticationServer/AuthenticationServer.Web.csproj", "AuthenticationServer/"]
COPY ["AuthenticationServer.Infrastructure/AuthenticationServer.Infrastructure.csproj", "AuthenticationServer.Infrastructure/"]
COPY ["AuthenticationServer.Domain/AuthenticationServer.Domain.csproj", "AuthenticationServer.Domain/"]
COPY ["Authentication.Persistance/Authentication.Persistance.csproj", "Authentication.Persistance/"]
COPY ["AuthenticationServer.Common/AuthenticationServer.Common.csproj", "AuthenticationServer.Common/"]
COPY ["AuthenticationServer.Service/AuthenticationServer.Service.csproj", "AuthenticationServer.Service/"]
COPY ["AuthenticationServer.Logic/AuthenticationServer.Logic.csproj", "AuthenticationServer.Logic/"]
RUN dotnet restore "AuthenticationServer/AuthenticationServer.Web.csproj"
COPY . .
WORKDIR "/src/AuthenticationServer"
RUN dotnet build "AuthenticationServer.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthenticationServer.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthenticationServer.Web.dll"]