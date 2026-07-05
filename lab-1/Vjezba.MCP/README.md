# Vjezba MCP Server

MCP server koji izlaže podatke aplikacije Vjezba AI agentima.

## Dostupni alati
- GetAllRadnaOprema - dohvati sve radne opreme
- GetRadnaOpremaById - dohvati opremu po ID-u
- GetAllRadnici - dohvati sve radnike
- GetAllLokacije - dohvati sve lokacije
- GetAllOdrzavanja - dohvati sva održavanja
- SearchRadnaOprema - pretraži opremu

## Pokretanje
Aplikacija mora biti pokrenuta na https://localhost:7001

dotnet run --project Vjezba.MCP/Vjezba.MCP.csproj

## VS Code integracija
MCP server je konfiguriran u .vscode/mcp.json
