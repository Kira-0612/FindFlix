namespace FindFlix.Models;

using System.Text.Json.Serialization;

public class Platform
{
    public int Id { get; set; }
    public string? Name { get; set; }
}
public class IgdbGame
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public double? Rating { get; set; }

    [JsonPropertyName("first_release_date")]
    public long? FirstReleaseDate { get; set; }
    public string? Summary { get; set; }
    public List<Platform>? Platforms { get; set; }

}

