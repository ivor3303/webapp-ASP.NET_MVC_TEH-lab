FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Vjezba.App/Vjezba.App.csproj", "Vjezba.App/"]
COPY ["Vjezba.Model/Vjezba.Model.csproj", "Vjezba.Model/"]
RUN dotnet restore "Vjezba.App/Vjezba.App.csproj"
COPY . .
WORKDIR "/src/Vjezba.App"
RUN dotnet build "Vjezba.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vjezba.App.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vjezba.App.dll"]
