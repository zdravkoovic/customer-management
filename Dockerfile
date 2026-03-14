FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY Customer.sln .
COPY src/Customer.Core/Customer.Core.csproj src/Customer.Core/
COPY src/Customer.Application/Customer.Application.csproj src/Customer.Application/
COPY src/Customer.Infrastructure/Customer.Infrastructure.csproj src/Customer.Infrastructure/
COPY src/Customer.API/Customer.API.csproj src/Customer.API/

# This command downloads the necessarily package
# This is deliberately placed before copying the full source because dependencies will be cached in Docker and won't be downloaded each time
RUN dotnet restore

COPY src ./src
WORKDIR /app/src/Customer.API
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT [ "dotnet", "Customer.API.dll" ]