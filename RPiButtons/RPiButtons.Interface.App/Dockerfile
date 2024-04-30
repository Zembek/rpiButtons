FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

# Copy everything
COPY RPiButtons.Interface.App ./RPiButtons.Interface.App
COPY RPiButtons.MatrixButtons ./RPiButtons.MatrixButtons
COPY RPiButtons.SSD1306 ./RPiButtons.SSD1306

#Change dir
WORKDIR /App/RPiButtons.Interface.App

# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/RPiButtons.Interface.App/out .

ENTRYPOINT ["dotnet", "RPiButtons.Interface.App.dll"]