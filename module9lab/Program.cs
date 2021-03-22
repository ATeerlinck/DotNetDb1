using System;
using NLog.Web;
using System.IO;
using System.Linq;
namespace module9lab
{
    class Program
    {
        // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");
            string choice = "";
            string scrubbedFile = FileScrubber.ScrubMovies("movies.csv");
            logger.Info(scrubbedFile);
            MovieFile movieFile = new MovieFile(scrubbedFile);
            do{
                Console.WriteLine("1) List movies");
                Console.WriteLine("2) Add a movie");
                Console.WriteLine("3) Search movies");
                Console.WriteLine("Enter any other key to exit.");
                choice = Console.ReadLine();
                logger.Info("User choice: \""+choice+"\"");

                if (choice == "1"){
                    if (File.Exists(scrubbedFile)){
                        movieFile = new MovieFile(scrubbedFile);
                        foreach(Movie movie in movieFile.Movies){
                                Console.WriteLine(movie.Display());
                            }
                    }
                    else{
                        Console.WriteLine("File does not exist");
                    }
                }
                else if (choice == "2"){
                    Movie movie = new Movie();
                    Console.WriteLine("Enter a movie name and year (Name (year)).");
                    movie.title = Console.ReadLine();
                    if(movieFile.isUniqueTitle(movie.title)){
                        string input;
                        do{
                        Console.WriteLine("Enter a genre for this movie or press enter if you are done");
                        input = Console.ReadLine();
                        if (input!=""&&input.Length>3){
                            movie.genres.Add(input);
                        }
                        } while(input!="");
                    }
                    if (movie.genres.Count==0){
                        movie.genres.Add("(no genres listed)");
                    }
                    Console.WriteLine("Enter the director");
                    movie.director = Console.ReadLine();
                    if(movie.director.Length<2){
                        movie.director = "unassigned";
                    }
                    Console.WriteLine("Enter the movie's runtime (hours:minutes:seconds)");
                    try{
                        movie.runningTime = TimeSpan.Parse(Console.ReadLine());
                    }
                    catch{
                        movie.runningTime = new TimeSpan(0);
                    }
                    movieFile.AddMovie(movie);
                    
                }
                else if(choice == "3"){
                    Console.WriteLine("What do you want to search by?\n  1)Name\n  2)Year\n  3)Genre\n");
                    string answer = Console.ReadLine();
                    Console.WriteLine("Enter your search term:");
                    string search = Console.ReadLine();
                    if(answer == "1"){
                        var Movies = movieFile.Movies.Where(m => m.title.Contains(search));
                        var count = movieFile.Movies.Where(m => m.title.Contains(search)).Count();
                        //count display
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write($"There are ");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write(count);
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(" movies with ");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write(search);
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(" in their title.\n");
                        //movie list display
                        foreach(var t in Movies){
                            Console.WriteLine($"  {t.title}");
                        }
                        Console.ResetColor();
                    }
                    else if(answer == "2"){
                        var Movies = movieFile.Movies.Where(m => m.title.Contains(search));
                        var count = movieFile.Movies.Where(m => m.title.Contains(search)).Count();
                        //count display
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write($"There are ");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write(count);
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(" movies from ");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write(search);
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine(".");
                        //movie list display
                        foreach(var t in Movies){
                            Console.WriteLine($"  {t.title}");
                        }
                        Console.ResetColor();
                    }
                    else if(answer == "3"){
                        var Movies = movieFile.Movies.Where(m => m.genres.Contains(search));
                        var count = movieFile.Movies.Where(m => m.genres.Contains(search)).Count();
                        //count display
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write($"There are ");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write(count);
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(" movies in the ");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write(search);
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(" genre.\n");
                        //movie list display
                        foreach(var t in Movies){
                            Console.WriteLine($"  {t.title}");
                        }
                        Console.ResetColor();
                    }
                }
            } while (choice == "1" || choice == "2"|| choice == "3");

            logger.Info("Program ended");
        }
    }
}
