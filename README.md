# PulsePoint � H�lsouppf�ljningssystem f�r arbetsplatser
PulsePoint �r en webbaserad applikation f�r att registrera och f�lja upp h�lsodata f�r anst�llda. 
Systemet st�djer olika roller s�som anv�ndare, managers och administrat�rer, 
och inneh�ller funktioner som inloggning, h�lsoregistrering, arbetsplatshantering och aggregerad statistik per dag.

## Tekniker
- Backend: ASP.NET Core 9 Web API
- Databas MySQL
- Autentisering: ASP.NET Identity + JWT
- Struktur: Modell -> Controller -> Service -> Repository

## API Endpoints

| Metod | URL | Beskrivning | Roll |
|--|--|--|--|
| GET | /api/workplaces | H�mta alla arbetsplatser | Alla |
| POST | /api/workplaces | Skapa ny arbetsplats | Admin |
| PUT | /api/workplaces/{id} | Uppdatera arbetsplats | Admin |
| DELETE | /api/workpaces/{id} | Ta bort arbetsplats | Admin |
| POST | /api/auth/register | Registrera ny anv�ndare | - |
| POST | /api/auth/login | Logga in och f� JWT-token | - |
| GET | /api/auth/me | H�mta information om inloggad anv�ndare | Inloggad |
| GET | /api/healthentries | H�mta alla entries f�r inloggad anv�ndare | User |
| GET | /api/healthentries/{id} | H�mta specifik entry | User |
| POST | /api/healthentries | Skapa ny h�lsoregistrering | User |
| PUT | /api/healthentries/{id} | Uppdatera befintlig entry | User |
| DELETE | /api/healthentries/{id} | Ta bort en entry | User |
| GET | /api/healthentries/stats/daily | H�mta daglig statistik f�r arbetsplats | Manager |