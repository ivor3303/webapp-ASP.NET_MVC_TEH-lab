# Vjezba App - Evidencija održavanja radne opreme

## Demo pristupni podaci
- URL: https://localhost:7001
- Email: admin@vjezba.hr
- Lozinka: Admin123!
- Rola: Admin (može kreirati, uređivati i brisati sve)

## Konfiguracija API ključeva
Za AI funkcionalnosti potreban je Anthropic API ključ.
Pogledaj Vjezba.App/USER_SECRETS.md za upute.

Bez API ključa AI funkcionalnosti vraćaju placeholder poruku, ali aplikacija radi normalno.

## AI Asistent
Dostupan na: /ai-asistent
Omogućuje unos podataka prirodnim jezikom - npr:
"Dodaj radnu opremu: bušilica Bosch, inventarni broj INV-123, serijski broj SN-456"

## Manager korisnik
- Za kreiranje Manager korisnika: registrirajte se i zamolite admina da vam dodijeli rolu
