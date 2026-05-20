FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY CoreFlow.sln ./
COPY src/CoreFlow.Api/CoreFlow.Api.csproj src/CoreFlow.Api/
COPY src/CoreFlow.Application/CoreFlow.Application.csproj src/CoreFlow.Application/
COPY src/CoreFlow.Domain/CoreFlow.Domain.csproj src/CoreFlow.Domain/
COPY src/CoreFlow.Infrastructure/CoreFlow.Infrastructure.csproj src/CoreFlow.Infrastructure/

RUN dotnet restore

COPY . .

RUN dotnet publish src/CoreFlow.Api/CoreFlow.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "CoreFlow.Api.dll"]