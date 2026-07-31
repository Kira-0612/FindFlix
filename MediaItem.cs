public enum MediaType
{
    Game,
    Movie
}
class MediaItem
{
    public string Title { get; set; }
    public int Year { get; set; }
    public MediaType Kind { get; set; }

    private double rating;
    public double Rating
    {
        get { return rating; }
        set
        {
            if (value < 1.0 || value > 10.0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(Rating), $" Your Rating has to be between 1.0 and 10.0. Your Rating: {value}"
                );
            }
            rating = value;
        }
    }
    public MediaItem(string title, int year, MediaType kind, double rating)
    {
        this.Title = title;
        this.Year = year;
        this.Kind = kind;
        this.Rating = rating;
    }

    public void Print()
    {
        Console.WriteLine($"Title: {Title}\nYear: {Year}\nKind: {Kind}\nRating: {Rating}\n");
    }

}