#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EventsApiTests/EventsApiTests.csproj", "EventsApiTests/"]
COPY ["EventsApi/EventsApi.csproj", "EventsApi/"]
RUN dotnet restore "EventsApiTests/EventsApiTests.csproj"
COPY . .
WORKDIR "/src/EventsApiTests"
RUN dotnet build "EventsApiTests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventsApiTests.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventsApiTests.dll"]
