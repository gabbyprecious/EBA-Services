FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as build
WORKDIR /app

COPY . .

# Copy csproj and restore as distinct layers
# COPY *.csproj ./
RUN dotnet restore

RUN dotnet publish -o /app/published-app
# Copy everything else and build
# RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine as runtime

WORKDIR /app

# COPY --from=build /app/out .
COPY --from=build /app/published-app /app

ENTRYPOINT ["dotnet", "/app/FinquestTest.dll"]