FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
COPY ["TweetServices/Kwetter.TweetService/Kwetter.TweetService.csproj", "Kwetter.TweetService/"]
COPY ["TweetServices/Kwetter.TweetService.Persistence/Kwetter.TweetService.Persistence.csproj", "Kwetter.TweetService.Persistence/"]

RUN dotnet restore "Kwetter.TweetService/Kwetter.TweetService.csproj"
COPY . .
WORKDIR "/TweetServices/Kwetter.TweetService"
RUN dotnet build "Kwetter.TweetService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kwetter.TweetService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kwetter.TweetService.dll"]