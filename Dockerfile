FROM mcr.microsoft.com/dotnet/aspnet:5.0
ARG AZR_CONN_STR=/dev/null

WORKDIR /app

COPY publish/ ./

CMD export ASPNETCORE_URLS=http://*:$PORT && export SQLCONNSTR_DefaultConnection="$AZR_CONN_STR" && dotnet Lasaro.ExchangeRate.API.dll
