# Deployment - Vjezba App

## Railway.app Deploy (preporučeno - besplatno)

### Koraci
1. Idi na https://railway.app i prijavi se s GitHub računom
2. Klikni "New Project" -> "Deploy from GitHub repo"
3. Odaberi repozitorij: ivor3303/webapp-ASP.NET_MVC_TEH-lab
4. Root Directory ostavi prazan/na korijenu repozitorija - railway.json (u korijenu) referencira lab-1/Vjezba.App/Dockerfile
5. Railway automatski detektira konfiguraciju iz railway.json i deploya
6. U Settings -> Variables dodaj:
   - ASPNETCORE_ENVIRONMENT = Production
   - Anthropic__ApiKey = tvoj-api-key (opcionalno)
7. Nakon deploya dobiješ javni URL npr: https://vjezba-app.railway.app

### Napomena
Aplikacija koristi SQLite bazu koja se resetira pri svakom deployu.
Za produkcijsku bazu koristiti Railway PostgreSQL addon ili Azure SQL.

## Status deploya
Aplikacija je pripremljena za Azure deploy.
Dockerfile i GitHub Actions workflow su konfigurirani.
Za aktivaciju deploya potrebno je:
1. Kreirati Azure App Service (upute niže)
2. Dodati AZURE_WEBAPP_PUBLISH_PROFILE secret u GitHub repo Settings > Secrets

## Lokalno pokretanje
dotnet run --project lab-1/Vjezba.App/Vjezba.App.csproj
Aplikacija dostupna na: https://localhost:7001

## Azure App Service Deployment

### Prerequisites
- Azure account (free tier works)
- Azure CLI installed

### Steps

1. Create Azure App Service:
```
az login
az group create --name vjezba-rg --location westeurope
az appservice plan create --name vjezba-plan --resource-group vjezba-rg --sku F1 --is-linux
az webapp create --name vjezba-app --resource-group vjezba-rg --plan vjezba-plan --runtime "DOTNETCORE|9.0"
```

2. Get publish profile:
```
az webapp deployment list-publishing-profiles --name vjezba-app --resource-group vjezba-rg --xml
```

3. Add publish profile as GitHub secret:
- Go to GitHub repo Settings > Secrets > Actions
- Add secret: AZURE_WEBAPP_PUBLISH_PROFILE
- Paste the publish profile XML

4. Push to main branch to trigger deployment

### Local Docker testing
```
docker build -t vjezba-app -f Vjezba.App/Dockerfile .
docker run -p 8080:8080 vjezba-app
```
