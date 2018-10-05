#!/bin/sh

cd HaruGaKita
dotnet ef database update
cd ../HaruGaKita.Test
dotnet test