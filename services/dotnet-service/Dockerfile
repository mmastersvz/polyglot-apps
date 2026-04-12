FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY ./publish/ ./

USER app

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "DotnetService.dll"]