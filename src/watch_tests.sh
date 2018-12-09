#!/bin/sh

cd HaruGaKita.WebAPI
ASPNETCORE_ENVIRONMENT=Test dotnet ef database update
cd ../HaruGaKita.Test
ASPNETCORE_ENVIRONMENT=Test dotnet watch test 