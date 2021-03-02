using System;
using NLog.Web;
using System.IO;

namespace module7assignment
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
                Console.WriteLine("Enter any other key to exit.");
                choice = Console.ReadLine();
                logger.Info("User choice: \""+choice+"\"");

                if (choice == "1"){
                    if (File.Exists(scrubbedFile)){
                        movieFile = new MovieFile(scrubbedFile);
                        foreach(Movie movie in movieFile.Movies){
                                Console.WriteLine(movie);
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
                        Console.WriteLine("Enter a genre for this movie");
                        input = Console.ReadLine();
                        if (input!="done"&&input.Length>0){
                            movie.genres.Add(input);
                        }
                        } while(input!="done");
                    }
                    if (movie.genres.Count==0){
                        movie.genres.Add("(no genres listed)");
                    }
                    Console.WriteLine("Enter the director");
                    movie.director = Console.ReadLine();
                    if(movie.director.Length<2){
                        movie.director = "unassigned";
                    }
                    Console.WriteLine("Enter the movie's runtime");
                    try{
                        movie.runningTime = TimeSpan.Parse(Console.ReadLine());
                    }
                    catch{
                        movie.runningTime = new TimeSpan(0);
                    }
                    movieFile.AddMovie(movie);
                }
            } while (choice == "1" || choice == "2");

            logger.Info("Program ended");
        }
    }
}
