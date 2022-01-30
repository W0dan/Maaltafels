FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /src
COPY *.sln ./
COPY Lalena.Domain/*.csproj ./Lalena.Domain
COPY Lalena.Api/*.csproj ./Lalena.Api

RUN dotnet restore
COPY . .

WORKDIR /src/Lalena.Domain
RUN dotnet build -c Release -o /app

WORKDIR /src/Lalena.Api
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base as final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Lalena.Api.exe"]