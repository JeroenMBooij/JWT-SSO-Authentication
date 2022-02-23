#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AuthenticationServer.Web/AuthenticationServer.Web.csproj", "AuthenticationServer.Web/"]
COPY ["AuthenticationServer.Services/AuthenticationServer.Services.csproj", "AuthenticationServer.Services/"]
COPY ["AuthenticationServer.Domain/AuthenticationServer.Domain.csproj", "AuthenticationServer.Domain/"]
COPY ["Authentication.Persistance/Authentication.Persistance.csproj", "Authentication.Persistance/"]
COPY ["AuthenticationServer.Common/AuthenticationServer.Common.csproj", "AuthenticationServer.Common/"]
COPY ["AuthenticationServer.Logic/AuthenticationServer.Logic.csproj", "AuthenticationServer.Logic/"]
RUN dotnet restore "AuthenticationServer.Web/AuthenticationServer.Web.csproj"
COPY . .
WORKDIR "/src/AuthenticationServer.Web"
RUN dotnet build "AuthenticationServer.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthenticationServer.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthenticationServer.Web.dll"]
