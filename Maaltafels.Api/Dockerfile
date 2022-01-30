FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet publish Maaltafels.Api -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=build-env Maaltafels.Api/out .

ENTRYPOINT ["dotnet", "Maaltafels.Api.exe"]