using System;
using System.IO;
using NLog.Web;
using System.Text.RegularExpressions;

namespace module4assignment
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory() + "\\nlog.config";
            var logger = NLog.Web.NLogBuilder.ConfigureNLog(path).GetCurrentClassLogger();
            string movies = "movies.csv";
            string choice = "";
            int ID = 0;
            do{
                Console.WriteLine("1) Add a movie");
                Console.WriteLine("2) List movies");
                Console.WriteLine("Enter any other key to exit.");
                choice = Console.ReadLine();
                if (choice == "2"){
                    if (File.Exists(movies))
                    {
                        StreamReader sr = new StreamReader(movies);
                        while (!sr.EndOfStream)
                        {
                            bool logged = false;
                            string line = sr.ReadLine();
                            string[] movie = line.Split(",");
                            string nameBackup = movie[1];
                            try {
                                int n = Int32.Parse(movie[0]);
                            }
                            catch (Exception ex){
                                logger.Error(ex.Message);
                                logger.Warn("Not an Id. Probably a header or misinput");
                                logged = true;
                            }
                            try
                            {
                            
                            movie[1] = line.Substring(line.IndexOf("\"")+1,line.LastIndexOf("\"")-8); //I don't know why LastIndexOf doesn't like working for the backend quote, but it would always give me an offset of characters.
                            }
                            catch
                            {
                            movie[1] = nameBackup;
                            }
                            if(logged){}
                            else{
                                int length = movie.Length;
                                string[] genres = movie[length-1].Split("|");
                                movie[length-1] = "";
                                for(int i = 0; i<genres.Length; i++){
                                    movie[length-1] += genres[i];
                                    if(genres.Length >i-1 && i!=genres.Length-1){
                                        movie[length-1] += ", ";
                                    }
                                    if(i==genres.Length-2){
                                        movie[length-1] += "and ";
                                    }
                                }
                                Console.WriteLine($"MovieID: {movie[0]} \nMovie Name: {movie[1]} \nGenres: {movie[length-1]} \n");
                            }
                        }
                        sr.Close();
                    }
                    else{
                        logger.Error("File Does not exist");
                    }
                }
                else if(choice=="1"){
                    while (true)
                    {
                        Console.WriteLine("Enter a new movie (Y/N)?");
                        string answer = Console.ReadLine().ToUpper();
                        if (answer != "Y") { break; }
                        Console.WriteLine("Enter the movie name and year in the format \"Name (Year)\". No Quotes please");
                        string name = Console.ReadLine();
                        Console.WriteLine("Enter the movie's genres(if it has several, split them with a \"|\". If it has no genres, put nothing in)"); 
                        string genres = Console.ReadLine();
                        if(genres==null||genres==""){
                            genres="(no genres listed)";
                        }
                        StreamReader sr = new StreamReader(movies);
                        while (!sr.EndOfStream)
                        {
                            bool logged = false;
                            string line = sr.ReadLine();
                            string[] movie = line.Split(",");
                            try {
                                int n = Int32.Parse(movie[0]);
                            }
                            catch {
                                logged = true;
                            }
                            if(logged){}
                            else{ID = Int32.Parse(movie[0]+1);}
                        }
                        sr.Close();
                        StreamWriter sw = new StreamWriter(movies, true);
                        sw.WriteLine("{0},\"{1}\",{2}", ID, name, genres);
                        sw.Close();
                    }
                }
            }while (choice == "1" || choice == "2");
        }
    }
}
