# Deployment Instructions

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
docker run -p 8080:80 vjezba-app
```
