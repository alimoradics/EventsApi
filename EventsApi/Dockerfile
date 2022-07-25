#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EventsApi/EventsApi.csproj", "EventsApi/"]
RUN dotnet restore "EventsApi/EventsApi.csproj"
COPY . .
WORKDIR "/src/EventsApi"
RUN dotnet build "EventsApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventsApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventsApi.dll"]
