# Vjezba App - Evidencija održavanja radne opreme

## 🌐 Live aplikacija
**URL: https://webapp-aspnetmvcteh-lab-production.up.railway.app**

## 👤 Demo pristupni podaci
| Rola | Email | Lozinka |
|------|-------|---------|
| Admin | admin@vjezba.hr | Admin123! |

## 🚀 Kako demonstrirati profesoru

### 1. CRUD demonstracija
1. Otvori https://webapp-aspnetmvcteh-lab-production.up.railway.app
2. Klikni "Prijava" u navigaciji
3. Upiši email: admin@vjezba.hr, lozinka: Admin123!
4. Klikni "Prijava"
5. Idi na "Radna oprema" → klikni "Dodaj"
6. Ispuni formu i spremi → demonstrira CREATE
7. Klikni "Uredi" na zapisu → demonstrira UPDATE
8. Klikni "Obriši" → demonstrira DELETE

### 2. AI Asistent demonstracija
1. Idi na /ai-asistent ili klikni "✨ AI Asistent" u navigaciji
2. Upiši: "Dodaj radnu opremu: bušilica Bosch, inventarni broj INV-123, serijski broj SN-456"
3. Klikni "Analiziraj s AI"
4. AI popunjava formu automatski
5. Klikni "Spremi opremu" za kreiranje zapisa

### 3. Global Search demonstracija
1. U navigaciji pronađi search box
2. Upiši "Bosch" i pritisni Enter
3. Prikazuju se rezultati iz svih entiteta

### 4. API demonstracija
Otvori u browseru:
- GET https://webapp-aspnetmvcteh-lab-production.up.railway.app/api/radnaOprema
- GET https://webapp-aspnetmvcteh-lab-production.up.railway.app/api/radnik
- GET https://webapp-aspnetmvcteh-lab-production.up.railway.app/api/lokacija
- POST https://webapp-aspnetmvcteh-lab-production.up.railway.app/api/ai/generate-maintenance

### 5. MCP demonstracija (VS Code)
1. Otvori projekt u VS Code s Claude ekstenzijom
2. Pokreni aplikaciju lokalno: cd lab-1/Vjezba.App && dotnet run
3. MCP server se automatski spaja (konfiguracija: .vscode/mcp.json)
4. U Claude chatu upiši: "Dohvati sve radne opreme iz sustava"
5. Claude koristi MCP alat i prikazuje podatke

### 6. Playwright testovi
cd lab-1/Vjezba.PlaywrightTests
npm install
npx playwright install chromium
npm test

## 📋 Implementirane funkcionalnosti
- ✅ ASP.NET Core MVC s Entity Framework Core (SQLite)
- ✅ ASP.NET Core Identity - lokalna prijava i Google OAuth
- ✅ Role: Admin i Manager
- ✅ CRUD za sve entitete (soft delete)
- ✅ REST API za sve entitete
- ✅ Global Search
- ✅ AI Asistent (/ai-asistent)
- ✅ Dropzone file upload
- ✅ Serilog logging
- ✅ Responsive UI
- ✅ Playwright testovi (24 testa)
- ✅ MCP server za agentic IDE
- ✅ Deploy na Railway cloud

## 🔧 Lokalno pokretanje
cd lab-1/Vjezba.App
dotnet run
Aplikacija dostupna na: https://localhost:7001

## 🌐 API endpointi
- GET/POST /api/radnaOprema
- GET/PUT/DELETE /api/radnaOprema/{id}
- GET/POST /api/radnik
- GET/PUT/DELETE /api/radnik/{id}
- GET/POST /api/lokacija
- GET/PUT/DELETE /api/lokacija/{id}
- GET/POST /api/proizvodac
- GET/PUT/DELETE /api/proizvodac/{id}
- GET/POST /api/kategorijaOpreme
- GET/PUT/DELETE /api/kategorijaOpreme/{id}
- GET/POST /api/odrzavanje
- GET/PUT/DELETE /api/odrzavanje/{id}
- GET/POST /api/servisniZahtjev
- GET/PUT/DELETE /api/servisniZahtjev/{id}
- GET/POST /api/zaduzenjeOpreme
- GET/PUT/DELETE /api/zaduzenjeOpreme/{id}
- POST /api/ai/generate-maintenance
- POST /api/ai/generate-service-request

## 🔑 AI konfiguracija
Za AI funkcionalnosti potreban je Anthropic API ključ:
cd lab-1/Vjezba.App
dotnet user-secrets set "Anthropic:ApiKey" "sk-ant-vas-kljuc"
Bez ključa AI vraća placeholder poruku, aplikacija radi normalno.
