#!/bin/bash
git pull
dotnet RPiButtons/RPiButtons.Interface.App/RPiButtons.Interface.App.csproj -c Release -r linux-arm
dotnet RPiButtons/RPiButtons.Interface.App/bin/Release/net8.0/linux-arm/RPiButtons.Interface.App.dll
