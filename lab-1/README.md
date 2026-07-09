# Vjezba App - Evidencija održavanja radne opreme

## 🌐 Live aplikacija
URL: https://webapp-aspnetmvcteh-lab-production.up.railway.app
Demo login: admin@vjezba.hr / Admin123!

## Demo pristupni podaci
- URL lokalno: https://localhost:7001
- Email: admin@vjezba.hr
- Lozinka: Admin123!
- Rola: Admin (puni pristup - CRUD, brisanje, uređivanje)

## Pokretanje aplikacije
cd lab-1/Vjezba.App
dotnet run

## Funkcionalnosti
- CRUD za radnu opremu, radnike, lokacije, proizvođače, kategorije, održavanja, servisne zahtjeve
- ASP.NET Core Identity - lokalna registracija i Google OAuth
- Role: Admin i Manager
- Global Search (/search)
- AI Asistent (/ai-asistent) - unos podataka prirodnim jezikom
- Dropzone file upload
- Serilog logging (Logs/ folder)
- Responsive UI
- REST API za sve entitete (/api/*)
- MCP server za agentic IDE integraciju

## API endpointi
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

## AI konfiguracija
cd lab-1/Vjezba.App
dotnet user-secrets set "Anthropic:ApiKey" "sk-ant-vaš-ključ"
Bez ključa AI vraća placeholder poruku, aplikacija radi normalno.

## MCP Server (Agentic IDE)
Konfiguracija: .vscode/mcp.json
Aplikacija mora biti pokrenuta na https://localhost:7001

### Dostupni MCP alati
- GetAllRadnaOprema
- GetRadnaOpremaById
- GetAllRadnici
- GetAllLokacije
- GetAllOdrzavanja
- SearchRadnaOprema

### Kako testirati MCP
1. Otvori VS Code s Claude ekstenzijom
2. Pokreni aplikaciju (dotnet run)
3. U Claude chatu upiši: "Dohvati sve radne opreme"
4. Claude koristi MCP alat i prikazuje podatke iz aplikacije

## Playwright testovi
cd lab-1/Vjezba.PlaywrightTests
npm install
npx playwright install chromium
npm test
(aplikacija mora biti pokrenuta)

## Deploy
Dockerfile i GitHub Actions workflow su konfigurirani.
Build artifact se generira automatski na svaki push na main granu.
Za cloud deploy pogledaj lab-1/DEPLOYMENT.md
