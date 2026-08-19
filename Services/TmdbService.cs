namespace FindFlix.Services;

using FindFlix.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Json;

public class TmdbService
{
    private string ApiKey;
    private HttpClient Client;
    public TmdbService(string apiKey)
    {
        this.ApiKey = apiKey;
        this.Client = new HttpClient();
    }
    public async Task<List<MediaItem>?> SearchMovies(string searchTerm)
    {
        if (searchTerm == null)
        {
            return null;
        }
        string movieUrl = $"https://api.themoviedb.org/3/search/movie?api_key={ApiKey}&query={searchTerm}";

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        HttpResponseMessage movieResponse = await Client.GetAsync(movieUrl);

        var movieJson = await movieResponse.Content.ReadAsStringAsync();
        TmdbSearchResult? movieResult = JsonSerializer.Deserialize<TmdbSearchResult>(movieJson, options);

        if (movieResult == null || movieResult.Results == null)
        {
            throw new Exception("No results found");
        }
        List<MediaItem> results = new List<MediaItem>();

        foreach (Movie m in movieResult.Results)
        {
            int releaseYear = (m.ReleaseDate != null && m.ReleaseDate.Length >= 4)
                ? int.Parse(m.ReleaseDate.Substring(0, 4))
                : 0;

            double rating = m.Rating != null
                ? Math.Clamp(Math.Round(m.Rating.Value, 1), 1, 10)
                : 1;

            results.Add(new(m.Title, releaseYear, MediaType.Movie, rating, null));
        }
        return results;
    }
}