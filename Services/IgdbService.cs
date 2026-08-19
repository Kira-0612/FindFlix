using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

public class IgdbService
{
    private string ClientId;
    private string ClientSecret;
    private HttpClient Client;
    private string? AccessToken;

    public IgdbService(string clientId, string clientSecret)
    {
        this.ClientId = clientId;
        this.ClientSecret = clientSecret;
        this.Client = new HttpClient();
    }
    public async Task<string> GetAccessToken()
    {
        string tokenUrl = $"https://id.twitch.tv/oauth2/token?client_id={ClientId}&client_secret={ClientSecret}&grant_type=client_credentials";

        HttpResponseMessage tokenResponse = await Client.PostAsync(tokenUrl, null);
        tokenResponse.EnsureSuccessStatusCode();
        string tokenJson = await tokenResponse.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        TwitchTokenResponse? tokenResult = JsonSerializer.Deserialize<TwitchTokenResponse>(tokenJson, options);

        if (tokenResult == null || tokenResult.AccessToken == null)
        {
            throw new Exception("No token found");
        }
        return tokenResult.AccessToken;
    }
    public async Task<List<MediaItem>?> SearchGames(string searchTerm)
    {
        if (searchTerm == null)
        {
            return null;
        }

        AccessToken = AccessToken == null
            ? await GetAccessToken()
            : AccessToken;

        Client.DefaultRequestHeaders.Clear();
        Client.DefaultRequestHeaders.Add("Client-ID", ClientId);
        Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AccessToken}");

        //der haupt Aufruf
        string query = ($"fields name, rating, first_release_date, summary, platforms.name;where name = \"{searchTerm}\"; limit 5;");
        StringContent content = new StringContent(query, Encoding.UTF8, "text/plain");

        HttpResponseMessage response = await Client.PostAsync("https://api.igdb.com/v4/games", content);
        string json = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        List<IgdbGame>? games = JsonSerializer.Deserialize<List<IgdbGame>>(json, options);
        if (games == null || games.Count == 0)
        {
            throw new Exception("No results found");
        }

        List<MediaItem> results = new List<MediaItem>();

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

            results.Add(new(g.Name, releaseYear, MediaType.Game, rating, platforms));
        }
        return results;
    }
}