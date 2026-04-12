FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY DotnetService.sln .
COPY src/DotnetService/DotnetService.csproj src/DotnetService/
RUN dotnet restore src/DotnetService/DotnetService.csproj
COPY src/ src/
RUN dotnet publish src/DotnetService/DotnetService.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "DotnetService.dll"]
