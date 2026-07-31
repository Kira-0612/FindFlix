using System;
using System.Linq;

List<MediaItem> MediaList = new List<MediaItem>();

MediaList.Add(new("Spiderman: No Way Home", 2021, MediaType.Movie, 8.2));
MediaList.Add(new("Oppenheimer", 2023, MediaType.Movie, 6.7));
MediaList.Add(new("Elemental", 2023, MediaType.Movie, 7.8));
MediaList.Add(new("Minecraft", 2009, MediaType.Game, 9.7));
MediaList.Add(new("GTA V", 2013, MediaType.Game, 8.7));

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