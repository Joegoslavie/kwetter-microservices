#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Following/Kwetter.FollowingService/Kwetter.FollowingService.csproj", "Following/Kwetter.FollowingService/"]
RUN dotnet restore "Following/Kwetter.FollowingService/Kwetter.FollowingService.csproj"
COPY . .
WORKDIR "/src/Following/Kwetter.FollowingService"
RUN dotnet build "Kwetter.FollowingService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kwetter.FollowingService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kwetter.FollowingService.dll"]