FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
COPY ["ProfileServices/Kwetter.ProfileService/Kwetter.ProfileService.csproj", "Kwetter.ProfileService/"]
COPY ["ProfileServices/Kwetter.ProfileService.Persistence/Kwetter.ProfileService.Persistence.csproj", "Kwetter.ProfileService.Persistence/"]

RUN dotnet restore "Kwetter.ProfileService/Kwetter.ProfileService.csproj"
COPY . .
WORKDIR "/Kwetter.ProfileService"
RUN dotnet build "Kwetter.ProfileService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kwetter.ProfileService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kwetter.ProfileService.dll"]