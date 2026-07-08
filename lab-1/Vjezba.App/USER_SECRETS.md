# Postavljanje API ključeva

## Anthropic API Key (za AI funkcionalnosti)

### Opcija 1: User Secrets (preporučeno za development)
cd lab-1/Vjezba.App
dotnet user-secrets set "Anthropic:ApiKey" "sk-ant-vaš-ključ-ovdje"

### Opcija 2: Environment variable
$env:Anthropic__ApiKey = "sk-ant-vaš-ključ-ovdje"

### Opcija 3: appsettings.json (NE commitati u git!)
{
  "Anthropic": {
    "ApiKey": "sk-ant-vaš-ključ-ovdje"
  }
}

## Google OAuth (za Google login)
cd lab-1/Vjezba.App
dotnet user-secrets set "Authentication:Google:ClientId" "vaš-client-id"
dotnet user-secrets set "Authentication:Google:ClientSecret" "vaš-client-secret"

## Provjera postavljenih secretsa
dotnet user-secrets list
