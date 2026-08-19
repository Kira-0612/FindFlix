**For German Version please see [README_DE.md](README_DE.md)

# FindFlix

FindFlix is a C# console application for searching games and movies and saving them to a personal library. The app connects to two external APIs (IGDB for games, TMDb for movies) and stores the results permanently in a local SQLite database.

This project was built as part of my learning path in C#, API integration, and databases.

## Features

- Search games via the IGDB API (platforms, release year, rating)
- Search movies via the TMDb API, with selection from multiple search results
- Persistent storage in a SQLite database using Entity Framework Core
- View library, sortable by rating, release year, or media type
- Delete entries
- Duplicate protection (same title + year won't be saved twice)
- Error handling for failed API requests

## Technologies

- C# / .NET (console application)
- Entity Framework Core with SQLite
- IGDB API (authentication via Twitch OAuth)
- TMDb API
- DotNetEnv for managing API keys through a `.env` file

## Project Structure

```
FindFlix/
├── Data/
│   └── AppDbContext.cs        # Database connection (EF Core)
├── Models/
│   ├── MediaItem.cs           # Data model for games and movies
│   ├── IgdbGame.cs            # IGDB API response model
│   ├── Movie.cs                # TMDb API response model
│   └── TwitchTokenResponse.cs # Twitch authentication response model
├── Services/
│   ├── IgdbService.cs         # Communication with the IGDB API
│   └── TmdbService.cs         # Communication with the TMDb API
├── Program.cs                  # Main menu and program flow
└── FindFlix.csproj
```

## Installation and Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/Kira-0612/FindFlix.git
   cd FindFlix
   ```

2. Install dependencies:
   ```bash
   dotnet restore
   ```

3. Create a `.env` file in the project root (see the API Keys section below):
   ```
   TWITCH_CLIENT_ID=your_client_id
   TWITCH_CLIENT_SECRET=your_client_secret
   TMDB_API_KEY=your_tmdb_api_key
   ```

4. Run the app:
   ```bash
   dotnet run
   ```

The SQLite database (`FindFlix.db`) is created automatically on first run.

## Usage

After starting, a menu appears:

```
1 - Search Game
2 - Search Movie
3 - Show Library
4 - Delete Entry
5 - Exit
```

- **1 / 2**: Enter a search term. For movies, you can then pick one of the results to save.
- **3**: Shows the saved library, with a choice of sorting by rating, year, or media type.
- **4**: Deletes an entry by its ID.
- **5**: Exits the program.

## Setting Up API Keys

The app requires two free API accounts.

### IGDB (Games)

IGDB uses Twitch authentication, so you need a Twitch developer app:

1. Go to [dev.twitch.tv/console/apps](https://dev.twitch.tv/console/apps) and log in with a Twitch account.
2. Click "Register Your Application".
3. Enter any name, use `http://localhost` as the OAuth Redirect URL, and select "Game Integration" as the category.
4. Once created, the Client ID is visible. Use "New Secret" to generate the Client Secret.
5. Add both values to your `.env` file (`TWITCH_CLIENT_ID`, `TWITCH_CLIENT_SECRET`).

### TMDb (Movies)

1. Create a free account at [themoviedb.org](https://www.themoviedb.org).
2. In your account settings, go to "API" and request a free API key (type "Developer", usage "Personal").
3. Add the generated API Key (v3 auth) to your `.env` file (`TMDB_API_KEY`).

The `.env` file contains personal credentials and should never be uploaded to a public repository. It is already excluded via `.gitignore` in this project.