public enum MediaType
{
    Game,
    Movie
}
public class MediaItem
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public int Year { get; set; }
    public MediaType Kind { get; set; }
    public List<string?>? Platforms { get; set; }
    private double rating;
    public double Rating
    {
        get { return rating; }
        set
        {
            if (value < 1.0 || value > 10.0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(Rating), $" Your Rating has to be between 1.0 and 10.0. Your Rating: {value}");
            }
            rating = value;
        }
    }
    public MediaItem(string? title, int year, MediaType kind, double rating, List<string?>? platforms = null)
    {
        this.Title = title;
        this.Year = year;
        this.Kind = kind;
        this.Rating = rating;
        this.Platforms = platforms;
    }
    public void Print()
    {
        Console.WriteLine($"{Id} - Title: {Title}\nYear: {Year}\nKind: {Kind}\nRating: {Rating}");
        if (Platforms != null && Platforms.Count > 0)
        {
            Console.WriteLine($"Platforms: {string.Join(", ", Platforms)}");
        }
        Console.WriteLine();
    }

}