using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

public class TmdbSearchResult
{
    [JsonPropertyName("results")]
    public List<Movie>? Results { get; set; }
}
public class Movie
{
    public string? Title { get; set; }

    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; set; }

    [JsonPropertyName("vote_average")]
    public double? Rating { get; set; }
}