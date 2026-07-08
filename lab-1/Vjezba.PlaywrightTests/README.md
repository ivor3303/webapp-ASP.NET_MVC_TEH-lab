# Playwright Tests - Vjezba App

## Pokriveni testovi
1. Homepage loads successfully
2. Navigation menu is visible
3. RadnaOprema list page loads
4. Radnik list page loads
5. Lokacija list page loads
6. Global search works
7. API endpoint returns JSON for RadnaOprema
8. API endpoint returns JSON for Radnik
9. API returns 404 for non-existing RadnaOprema
10. Login page loads
11. AI API generate-maintenance endpoint exists
12. AI Asistent page loads
13. API endpoint returns JSON for Lokacija
14. API endpoint returns JSON for Proizvodac
15. API endpoint returns JSON for KategorijaOpreme
16. API endpoint returns JSON for Odrzavanje
17. API endpoint returns JSON for ServisniZahtjev
18. API endpoint returns JSON for ZaduzenjeOpreme
19. API returns 404 for non-existing Radnik
20. Global search page loads with results
21. AI API generate-maintenance returns response
22. AI API generate-service-request returns response
23. AI API returns 400 for empty body
24. AI Asistent page has textarea and button

## Pokrivene API rute
- GET /api/radnaOprema
- GET /api/radnaOprema/{id}
- GET /api/radnik
- GET /api/radnik/{id}
- GET /api/lokacija
- GET /api/proizvodac
- GET /api/kategorijaOpreme
- GET /api/odrzavanje
- GET /api/servisniZahtjev
- GET /api/zaduzenjeOpreme
- POST /api/ai/generate-maintenance
- POST /api/ai/generate-service-request

## Pokrivene stranice
- / (homepage)
- /oprema
- /radnici
- /lokacije
- /search
- /ai-asistent
- /Identity/Account/Login

## Pokrenuti testove
cd Vjezba.PlaywrightTests
npm install
npx playwright install chromium
npm test
