FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 42317
EXPOSE 44302

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY HaruGaKita/HaruGaKita.csproj HaruGaKita/
RUN dotnet restore HaruGaKita/HaruGaKita.csproj
COPY . .
WORKDIR /src/HaruGaKita
RUN dotnet build HaruGaKita.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish HaruGaKita.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
#ENTRYPOINT ["dotnet", "run", "watch"]
