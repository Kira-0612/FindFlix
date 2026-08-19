# FindFlix

FindFlix ist eine Konsolen-App in C#, mit der man Spiele und Filme suchen und in einer eigenen Bibliothek speichern kann. Die App nutzt zwei externe APIs (IGDB für Spiele, TMDb für Filme) und speichert die Ergebnisse dauerhaft in einer lokalen SQLite-Datenbank.

Das Projekt ist im Rahmen meines Lernwegs in C#, API-Anbindung und Datenbanken entstanden.

## Features

- Spiele suchen über die IGDB-API (Plattformen, Erscheinungsjahr, Bewertung)
- Filme suchen über die TMDb-API, mit Auswahl aus mehreren Suchergebnissen
- Speicherung in einer SQLite-Datenbank über Entity Framework Core
- Bibliothek anzeigen, sortierbar nach Bewertung, Jahr oder Medientyp
- Einträge löschen
- Schutz vor doppelten Einträgen (gleicher Titel + Jahr wird nicht erneut gespeichert)
- Fehlerbehandlung bei fehlgeschlagenen API-Anfragen

## Technologien

- C# / .NET (Konsolen-App)
- Entity Framework Core mit SQLite
- IGDB API (Authentifizierung über Twitch OAuth)
- TMDb API
- DotNetEnv zur Verwaltung der API-Keys über eine `.env`-Datei

## Projektstruktur

```
FindFlix/
├── Data/
│   └── AppDbContext.cs        # Datenbankanbindung (EF Core)
├── Models/
│   ├── MediaItem.cs           # Datenmodell für Spiele und Filme
│   ├── IgdbGame.cs            # Antwortmodell der IGDB-API
│   ├── Movie.cs                # Antwortmodell der TMDb-API
│   └── TwitchTokenResponse.cs # Antwortmodell der Twitch-Authentifizierung
├── Services/
│   ├── IgdbService.cs         # Kommunikation mit der IGDB-API
│   └── TmdbService.cs         # Kommunikation mit der TMDb-API
├── Program.cs                  # Hauptmenü und Programmablauf
└── FindFlix.csproj
```

## Installation und Start

1. Repository klonen:
   ```bash
   git clone https://github.com/Kira-0612/FindFlix.git
   cd FindFlix
   ```

2. Abhängigkeiten installieren:
   ```bash
   dotnet restore
   ```

3. Eine `.env`-Datei im Projektordner anlegen (siehe Abschnitt API-Keys weiter unten):
   ```
   TWITCH_CLIENT_ID=dein_client_id
   TWITCH_CLIENT_SECRET=dein_client_secret
   TMDB_API_KEY=dein_tmdb_api_key
   ```

4. App starten:
   ```bash
   dotnet run
   ```

Die SQLite-Datenbank (`FindFlix.db`) wird beim ersten Start automatisch angelegt.

## Verwendung

Nach dem Start erscheint ein Menü:

```
1 - Search Game
2 - Search Movie
3 - Show Library
4 - Delete Entry
5 - Exit
```

- **1 / 2**: Suchbegriff eingeben. Bei Filmen kann anschließend aus den gefundenen Ergebnissen eins ausgewählt werden, das dann gespeichert wird.
- **3**: Zeigt die gespeicherte Bibliothek an, mit Auswahl der Sortierung nach Bewertung, Jahr oder Medientyp.
- **4**: Löscht einen Eintrag anhand seiner ID.
- **5**: Beendet das Programm.

## API-Keys einrichten

Die App braucht zwei kostenlose API-Zugänge.

### IGDB (Spiele)

IGDB läuft über Twitch-Authentifizierung, dafür braucht man eine Twitch-Developer-App:

1. Auf [dev.twitch.tv/console/apps](https://dev.twitch.tv/console/apps) mit einem Twitch-Account einloggen.
2. Auf "Register Your Application" klicken.
3. Einen beliebigen Namen eintragen, als OAuth Redirect URL reicht `http://localhost`, als Kategorie "Game Integration" wählen.
4. Nach dem Erstellen ist die Client-ID sichtbar. Über "New Secret" lässt sich das Client Secret generieren.
5. Beide Werte in die `.env`-Datei eintragen (`TWITCH_CLIENT_ID`, `TWITCH_CLIENT_SECRET`).

### TMDb (Filme)

1. Einen kostenlosen Account auf [themoviedb.org](https://www.themoviedb.org) erstellen.
2. In den Account-Einstellungen unter "API" einen kostenlosen API-Key beantragen (Typ "Developer", Verwendungszweck "Personal").
3. Den generierten API Key (v3 auth) in die `.env`-Datei eintragen (`TMDB_API_KEY`).

Die `.env`-Datei enthält persönliche Zugangsdaten und sollte nicht in ein öffentliches Repository hochgeladen werden. Sie ist in diesem Projekt bereits in der `.gitignore` ausgeschlossen.