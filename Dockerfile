FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY BasicGameBackend.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /out ./

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

VOLUME ["/app/db"]
RUN mkdir -p /app/db && chmod -R 777 /app/db

ENV ConnectionStrings__DefaultConnection="Data Source=/app/db/players.db"

ENTRYPOINT ["dotnet", "BasicGameBackend.dll"]
