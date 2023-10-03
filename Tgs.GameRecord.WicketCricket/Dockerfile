#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BuildConfiguration=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameRecord.WicketCricket/Tgs.GameRecord.WicketCricket.csproj", "Tgs.GameRecord.WicketCricket/"]
COPY ["Tgs.GameRecord.WicketCricket.Core/Tgs.GameRecord.WicketCricket.Core.csproj", "Tgs.GameRecord.WicketCricket.Core/"]
RUN dotnet restore "Tgs.GameRecord.WicketCricket/Tgs.GameRecord.WicketCricket.csproj"
COPY . .
WORKDIR "/src/Tgs.GameRecord.WicketCricket"
RUN dotnet build "Tgs.GameRecord.WicketCricket.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "Tgs.GameRecord.WicketCricket.csproj" -c $BuildConfiguration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameRecord.WicketCricket.dll"]