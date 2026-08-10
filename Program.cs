using System;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text;
using System.Dynamic;

List<MediaItem> MediaList = new List<MediaItem>();

MediaList.Add(new("Spiderman: No Way Home", 2021, MediaType.Movie, 8.2));
MediaList.Add(new("Oppenheimer", 2023, MediaType.Movie, 6.7));
MediaList.Add(new("Elemental", 2023, MediaType.Movie, 7.8));
MediaList.Add(new("Minecraft", 2009, MediaType.Game, 9.7));
MediaList.Add(new("GTA V", 2013, MediaType.Game, 9.0));

var Movies = MediaList.Where(n => n.Kind == MediaType.Movie).OrderByDescending(n => n.Rating);
var Games = MediaList.Where(n => n.Kind == MediaType.Game).OrderByDescending(n => n.Rating);


Console.WriteLine("------------------------");
Console.WriteLine("Movies from most Rated to least: ");
Console.WriteLine("------------------------");

foreach (MediaItem m in Movies)
{
    m.Print();
}

Console.WriteLine("------------------------");
Console.WriteLine("Games from most Rated to least: ");
Console.WriteLine("------------------------");

foreach (MediaItem g in Games)
{
    g.Print();
}

// zugangsdaten & API aufruf von IGDB
string clientId = "ENTFERNT";
string clientSecret = "ENTFERNT";

using HttpClient client = new HttpClient();

string tokenUrl = $"https://id.twitch.tv/oauth2/token?client_id={clientId}&client_secret={clientSecret}&grant_type=client_credentials";

HttpResponseMessage tokenResponse = await client.PostAsync(tokenUrl, null);
tokenResponse.EnsureSuccessStatusCode();
string tokenJson = await tokenResponse.Content.ReadAsStringAsync();


var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
TwitchTokenResponse? tokenResult = JsonSerializer.Deserialize<TwitchTokenResponse>(tokenJson, options);

if (tokenResult == null || tokenResult.AccessToken == null)
{
    throw new Exception("No token found");
}

string accessToken = tokenResult.AccessToken;


//Header für den aufruf von IGDB
client.DefaultRequestHeaders.Clear();
client.DefaultRequestHeaders.Add("Client-ID", clientId);
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

//der haupt aufruf
string query = ("fields name, rating, first_release_date, summary, platforms.name;where name = \"The Legend of Zelda: Breath of the Wild\"; limit 1;");
StringContent content = new StringContent(query, Encoding.UTF8, "text/plain");

HttpResponseMessage response = await client.PostAsync("https://api.igdb.com/v4/games", content);


string json = await response.Content.ReadAsStringAsync();
/*Console.WriteLine(json);

string responseBody = await response.Content.ReadAsStringAsync();
Console.WriteLine("Status: " + response.StatusCode);
Console.WriteLine("Body: " + responseBody);

response.EnsureSuccessStatusCode();*/

List<IgdbGame>? games = JsonSerializer.Deserialize<List<IgdbGame>>(json, options);
if (games == null || games.Count == 0)
{
    throw new Exception("No results found");
}
foreach (IgdbGame g in games) // initializing all games
{
    string platformNames = g.Platforms != null
    ? string.Join(",", g.Platforms.Select(p => p.Name))
    : "unknown";

    string releaseDate = g.FirstReleaseDate != null
        ? DateTimeOffset.FromUnixTimeSeconds(g.FirstReleaseDate.Value).Year.ToString()
        : "unknown";



    int releaseYear = g.FirstReleaseDate != null
        ? DateTimeOffset.FromUnixTimeSeconds(g.FirstReleaseDate.Value).Year
        : 0;

    double rating = g.Rating != null
        ? Math.Clamp(Math.Round(g.Rating.Value / 10, 1), 1, 10)
        : 1;

    List<string?>? platforms = g.Platforms?.Select(n => n.Name).ToList();


    MediaList.Add(new(g.Name, releaseYear, MediaType.Game, rating, platforms));




    Console.WriteLine($"Title: {g.Name} - Release Year: {releaseDate} - Media Type: {MediaType.Game} - Rating: {rating} - Plattforms: {platformNames}");
}

var apiKey = "ENTFERNT";

string queryMovie = "Oppenheimer";

string movieUrl = $"https://api.themoviedb.org/3/search/movie?api_key={apiKey}&query={queryMovie}";



HttpResponseMessage movieResponse = await client.GetAsync(movieUrl);
var movieJson = await movieResponse.Content.ReadAsStringAsync();

//Console.WriteLine(movieJson);

TmdbSearchResult? movieResult = JsonSerializer.Deserialize<TmdbSearchResult>(movieJson, options);

if (movieResult != null || movieResult.Results != null)
{
    throw new Exception("No results found");
}

foreach (Movie m in movieResult.Results)
{
    
}
