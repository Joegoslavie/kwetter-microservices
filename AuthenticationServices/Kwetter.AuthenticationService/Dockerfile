#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
#WORKDIR /src
COPY ["AuthenticationServices/Kwetter.AuthenticationService/Kwetter.AuthenticationService.csproj", "Kwetter.AuthenticationService/"]
RUN dotnet restore "Kwetter.AuthenticationService/Kwetter.AuthenticationService.csproj"
COPY . .
WORKDIR "/Kwetter.AuthenticationService"
RUN dotnet build "Kwetter.AuthenticationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kwetter.AuthenticationService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kwetter.AuthenticationService.dll"]