using DotNetEnv;

Env.Load();

List<MediaItem> MediaList = new List<MediaItem>();

AppDbContext appDbContext = new AppDbContext();

appDbContext.Database.EnsureCreated();

bool running = true;

String? ClientId = Environment.GetEnvironmentVariable("TWITCH_CLIENT_ID");
string? ClientSecret = Environment.GetEnvironmentVariable("TWITCH_CLIENT_SECRET");
string? apiKey = Environment.GetEnvironmentVariable("TMDB_API_KEY");

if (ClientId == null || ClientSecret == null || apiKey == null)
{
    Console.WriteLine("\nOne of the Secret keys is not available. Please check and try again later. ");
    return;
}
IgdbService igdbService = new IgdbService(ClientId, ClientSecret);
TmdbService tmdbService = new TmdbService(apiKey);

while (running)
{
    Console.WriteLine("1 - Search Game");
    Console.WriteLine("2 - Search Movie");
    Console.WriteLine("3 - Show Library");
    Console.WriteLine("4 - Delete Entry");
    Console.WriteLine("5 - Exit");

    string? choice = Console.ReadLine();
    Console.WriteLine($"\nYou chose Option {choice}");

    static void PrintInvalid()
    {
        Console.WriteLine("Invalid Input");
    }

    switch (choice)
    {
        case "1": //Searching for Games
            Console.Write("Which Game are you looking for? : ");
            string? searchedGame = Console.ReadLine();

            if (searchedGame == null)
            {
                break;
            }
            try
            {
                List<MediaItem>? foundGames = await igdbService.SearchGames(searchedGame);
                if (foundGames == null)
                {
                    break;
                }
                var DbGame = foundGames.OrderByDescending(n => n.Rating);

                foreach (MediaItem d in DbGame)
                {
                    d.Print();
                }

                foreach (MediaItem f in foundGames)
                {
                    bool exist = appDbContext.MediaItems.Any(n => n.Title == f.Title && n.Year == f.Year);
                    if (exist == false)
                    {
                        appDbContext.MediaItems.Add(f);
                    }
                }
                appDbContext.SaveChanges();
                break;
            }
            catch
            {
                Console.WriteLine("Searching the Game could not succsed. Please try again later.\n");
                break;
            }

        case "2": //Searching for Movies
            Console.Write("Which Movie are you looking for? : ");
            string? searchedMovie = Console.ReadLine();
            if (searchedMovie == null)
            {
                break;
            }

            try
            {
                List<MediaItem>? foundMovies = await tmdbService.SearchMovies(searchedMovie);
                if (foundMovies == null)
                {
                    break;
                }
                for (int i = 0; i < foundMovies.Count(); i++)
                {
                    MediaItem currMovie = foundMovies[i];
                    Console.Write($"{i + 1} - ");
                    currMovie.Print();
                }

                if (foundMovies == null || foundMovies.Any() == false)
                {
                    Console.WriteLine("\nNo Movies found.\n");
                    break;
                }

                Console.Write("Please write the number of the Movie you want to add to the library. => ");
                string? movieNr = Console.ReadLine();

                if (int.TryParse(movieNr, out int wantedMovieNr))
                {
                    MediaItem wantedMovie = foundMovies[wantedMovieNr - 1];

                    bool exist = appDbContext.MediaItems.Any(n => n.Title == wantedMovie.Title && n.Year == wantedMovie.Year);
                    if (exist == false)
                    {
                        appDbContext.MediaItems.Add(wantedMovie);
                    }

                    wantedMovie.Print();
                    appDbContext.SaveChanges();
                    break;
                }
                else
                {
                    PrintInvalid();
                    break;
                }
            }
            catch
            {
                Console.WriteLine("Searching the Movie could not succsed. Please try again later.\n");
                break;
            }
        case "3": //Showing Library
            if (appDbContext.MediaItems.Any() == false)
            {
                Console.WriteLine("The Library is empty.\n");
                break;
            }

            Console.Write("Choose a sorting option: \n 1 - By Rating\n 2 - By Year\n 3 - By MediaType\n Option: ");
            string? sortWay = Console.ReadLine();
            Console.WriteLine();
            if (sortWay == null)
            {
                PrintInvalid();
                break;
            }
            switch (sortWay)
            {
                case "1":
                    var DbR = appDbContext.MediaItems.OrderByDescending(n => n.Rating);
                    foreach (MediaItem d in DbR)
                    {
                        d.Print();
                    }
                    break;

                case "2":
                    var DbY = appDbContext.MediaItems.OrderByDescending(n => n.Year);
                    foreach (MediaItem d in DbY)
                    {
                        d.Print();
                    }
                    break;

                case "3":
                    var DbT = appDbContext.MediaItems.OrderByDescending(n => n.Kind);
                    foreach (MediaItem d in DbT)
                    {
                        d.Print();
                    }
                    break;
                default:
                    PrintInvalid();
                    break;
            }
            break;

        case "4": // Delete Entry
            var DbDelete = appDbContext.MediaItems.OrderByDescending(n => n.Rating);

            foreach (MediaItem d in DbDelete)
            {
                d.Print();
            }

            Console.Write("Which saved Madia Item(ID Nr.) do wou want to delete? : ");
            string? id = Console.ReadLine();
            if (id == null)
            {
                PrintInvalid();
                break;
            }

            if (int.TryParse(id, out int deleteId))
            {

                MediaItem? deleteItem = appDbContext.MediaItems.Find(deleteId);
                if (deleteItem == null)
                {
                    Console.WriteLine($"No Item with given ID = {deleteId} was found.\n");
                    break;
                }

                appDbContext.MediaItems.Remove(deleteItem);
                appDbContext.SaveChanges();
                Console.WriteLine($"The Item with ID Nr. = {deleteId} is successfully deleted.\n");
                break;
            }
            else
            {
                PrintInvalid();
                break;
            }
        case "5": // Exit
            running = false;
            break;

        default:
            PrintInvalid();
            break;
    }
}



