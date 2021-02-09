FROM mcr.microsoft.com/dotnet/aspnet:5.0

WORKDIR /app

COPY publish/ ./

CMD export ASPNETCORE_URLS=http://*:$PORT && dotnet Lasaro.ExchangeRate.API.dll
