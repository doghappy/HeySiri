FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /App

COPY . ./
RUN dotnet publish HeySiri.CLI -c Release

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /App
COPY --from=build-env /App/HeySiri.CLI/bin/Release/net7.0/publish .
ENTRYPOINT ["dotnet", "heysiri.dll"]
