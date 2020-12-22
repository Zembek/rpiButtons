#!/bin/bash
git pull
dotnet RPiButtons\RPiButtons.Interface.App/RPiButtons.Interface.App.csproj -c Release -r linux-arm
dotnet RPiButtons\RPiButtons.Interface.App/bin/Release/netcoreapp3.1/linux-arm/RPiButtons.Interface.App.dll
