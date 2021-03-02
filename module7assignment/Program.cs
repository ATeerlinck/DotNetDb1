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
                    
                }
                else if (choice == "2"){
                    
                }
            } while (choice == "1" || choice == "2");

            logger.Info("Program ended");
        }
    }
}
