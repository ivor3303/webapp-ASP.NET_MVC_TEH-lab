# Vjezba MCP Server - Upute za demonstraciju

## Što je MCP?
MCP (Model Context Protocol) omogućuje AI agentima (Claude u VS Code) da direktno pristupaju podacima aplikacije.

## Preduvjeti
1. Aplikacija mora biti pokrenuta na https://localhost:7001
2. VS Code s Claude ekstenzijom mora biti instaliran

## Kako pokrenuti MCP server
dotnet build lab-1/Vjezba.MCP/Vjezba.MCP.csproj

(MCP server se automatski pokreće kroz VS Code Claude ekstenziju)

## Kako testirati
1. Otvori VS Code
2. Otvori Claude chat (Claude ekstenzija)
3. MCP server se automatski spaja prema .vscode/mcp.json konfiguraciji
4. U Claude chatu upiši: "Dohvati sve radne opreme iz sustava"
5. Claude će koristiti GetAllRadnaOprema alat i prikazati podatke

## Dostupni alati (Tools)
- GetAllRadnaOprema - dohvati sve radne opreme
- GetRadnaOpremaById - dohvati opremu po ID-u (npr: "Dohvati opremu s ID 1")
- GetAllRadnici - dohvati sve radnike
- GetAllLokacije - dohvati sve lokacije
- GetAllOdrzavanja - dohvati sva održavanja
- SearchRadnaOprema - pretraži opremu (npr: "Pretraži opremu: bušilica")

## Primjeri upita za Claude
- "Koliko radnih oprema postoji u sustavu?"
- "Prikaži sve radnike"
- "Pretraži opremu koja sadrži Bosch"
- "Dohvati opremu s ID 1"
