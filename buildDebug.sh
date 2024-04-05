#!/bin/bash
git pull
dotnet build RPiButtons/RPiButtons.Interface.App/RPiButtons.Interface.App.csproj -c Debug -r linux-arm
dotnet RPiButtons/RPiButtons.Interface.App/bin/Debug/net8.0/linux-arm/RPiButtons.Interface.App.dll
