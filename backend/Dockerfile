FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-chiseled AS final

WORKDIR /app

COPY ./output/ .

ENTRYPOINT ["dotnet", "Portfolio.Api.dll"]
