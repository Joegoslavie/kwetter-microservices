#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Tweets/Kwetter.TweetService/Kwetter.TweetService.csproj", "Tweets/Kwetter.TweetService/"]
RUN dotnet restore "Tweets/Kwetter.TweetService/Kwetter.TweetService.csproj"
COPY . .
WORKDIR "/src/Tweets/Kwetter.TweetService"
RUN dotnet build "Kwetter.TweetService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kwetter.TweetService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kwetter.TweetService.dll"]