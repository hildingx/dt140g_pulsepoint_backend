# PulsePoint – Hälsouppföljningssystem för arbetsplatser
PulsePoint är en webbaserad applikation för att registrera och följa upp hälsodata för anställda. 
Systemet stödjer olika roller såsom användare, managers och administratörer, 
och innehåller funktioner som inloggning, hälsoregistrering, arbetsplatshantering och aggregerad statistik per dag.

## Tekniker
- Backend: ASP.NET Core 9 Web API
- Databas MySQL
- Autentisering: ASP.NET Identity + JWT
- Struktur: Modell -> Controller -> Service -> Repository

## API Endpoints

| Metod | URL | Beskrivning | Roll |
|--|--|--|--|
| GET | /api/workplaces | Hämta alla arbetsplatser | Alla |
| POST | /api/workplaces | Skapa ny arbetsplats | Admin |
| PUT | /api/workplaces/{id} | Uppdatera arbetsplats | Admin |
| DELETE | /api/workpaces/{id} | Ta bort arbetsplats | Admin |
| POST | /api/auth/register | Registrera ny användare | - |
| POST | /api/auth/login | Logga in och få JWT-token | - |
| GET | /api/auth/me | Hämta information om inloggad användare | Inloggad |
| GET | /api/healthentries | Hämta alla entries för inloggad användare | User |
| GET | /api/healthentries/{id} | Hämta specifik entry | User |
| POST | /api/healthentries | Skapa ny hälsoregistrering | User |
| PUT | /api/healthentries/{id} | Uppdatera befintlig entry | User |
| DELETE | /api/healthentries/{id} | Ta bort en entry | User |
| GET | /api/healthentries/stats/daily | Hämta daglig statistik för arbetsplats | Manager |