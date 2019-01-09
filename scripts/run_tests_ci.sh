#!/bin/sh

PROJECT_ROOT=`pwd`

pushd ${PROJECT_ROOT}/src/HaruGaKita.WebAPI
echo "Seeding database..."
ASPNETCORE_ENVIRONMENT=Test dotnet ef database update

echo "Running tests..."
pushd ${PROJECT_ROOT}/src/HaruGaKita.Test
ASPNETCORE_ENVIRONMENT=Test dotnet test