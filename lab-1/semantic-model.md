# Semantički model aplikacije

## Svrha aplikacije
Aplikacija **Evidencija odrzavanja radne opreme u zastiti na radu** sluzi za pracenje radne opreme, njezinih lokacija, proizvodaca i kategorija, kao i za upravljanje odrzavanjima, servisnim zahtjevima i zaduzenjima opreme. Cilj je imati centraliziranu evidenciju koja omogucuje pregled stanja opreme i operativnih aktivnosti.

## Entiteti i glavna svojstva

| Entitet | Glavna svojstva |
|---|---|
| RadnaOprema | Id, Naziv, InventarniBroj, SerijskiBroj, DatumNabave, Status, LokacijaId, ProizvodacId, KategorijaOpremeId |
| Radnik | Id, Ime, Prezime, RadnoMjesto, Email, Telefon, DatumZaposlenja, Aktivan |
| Lokacija | Id, Naziv, Adresa |
| Proizvodac | Id, Naziv, Drzava, KontaktEmail |
| KategorijaOpreme | Id, Naziv, Opis |
| Odrzavanje | Id, Datum, Opis, Cijena, IzvrsioId, TrajanjeTicks, OpremaId, Napomena |
| ServisniZahtjev | Id, DatumPrijave, OpisKvara, Hitno, OpremaId, Komentar |
| ZaduzenjeOpreme | Id, RadnikId, RadnaOpremaId, DatumZaduzenja, DatumRazduzenja |

## Odnosi između entiteta

| Izvorni entitet | Ciljni entitet | Vrsta odnosa |
|---|---|---|
| RadnaOprema | Lokacija | many-to-one |
| RadnaOprema | Proizvodac | many-to-one |
| RadnaOprema | KategorijaOpreme | many-to-one |
| Odrzavanje | RadnaOprema | many-to-one |
| Odrzavanje | Radnik | many-to-one |
| ServisniZahtjev | RadnaOprema | many-to-one |
| ZaduzenjeOpreme | Radnik | many-to-one |
| ZaduzenjeOpreme | RadnaOprema | many-to-one |

## Napomena
Svi odnosi u modelu su implementirani preko stranih kljuceva, a navigacijska svojstva sluze za pristup povezanim podacima unutar aplikacije i Entity Framework Core modela.
