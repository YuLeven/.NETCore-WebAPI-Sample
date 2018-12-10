FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /app

COPY HaruGaKita.Application/*.csproj ./HaruGaKita.Application/
COPY HaruGaKita.Common/*.csproj ./HaruGaKita.Common/
COPY HaruGaKita.Domain/*.csproj ./HaruGaKita.Domain/
COPY HaruGaKita.Persistence/*.csproj ./HaruGaKita.Persistence/
COPY HaruGaKita.WebAPI/*.csproj ./HaruGaKita.WebAPI/

WORKDIR /app/HaruGaKita.WebAPI
RUN dotnet restore

WORKDIR /app
COPY . ./

WORKDIR /app/HaruGaKita.WebAPI
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/HaruGaKita.WebAPI/out .
ENTRYPOINT ["dotnet", "HaruGaKita.WebAPI.dll"]
