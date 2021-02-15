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
                                if(movie.Length-2!=1){
                                    for(int i=2;i<movie.Length;i++){
                                        movie[1] += ","+movie[i];
                                    }
                                }
                                if(line.IndexOf("\"")!=line.LastIndexOf("\"")){
                                    int start = movie[1].IndexOf("\"")+1;
                                    int end = movie[1].LastIndexOf("\"")-1;
                                    movie[1] = movie[1].Substring(start, end); 
                                }
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
                        bool exists = false;
                        Console.WriteLine("Enter the movie name and year in the format \"Name (Year)\". No Quotes please");
                        string name = Console.ReadLine();                        
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
                            string nameBackup = movie[1];

                            try
                            {
                                if(movie.Length-2!=1){
                                    for(int i=2;i<movie.Length;i++){
                                        movie[1] += ","+movie[i];
                                    }
                                }
                                if(line.IndexOf("\"")!=line.LastIndexOf("\"")){
                                    int start = movie[1].IndexOf("\"")+1;
                                    int end = movie[1].LastIndexOf("\"")-1;
                                    movie[1] = movie[1].Substring(start, end); 
                                }
                            }
                            catch
                            {
                            movie[1] = nameBackup;
                            }
                            if(logged){}
                            
                            else if(movie[1].ToUpper()==name.ToUpper()){
                                Console.WriteLine("This movie already exists in this database. Your movie will not be added");
                                exists = true;
                                break;
                            }
                            else{
                                ID = Int32.Parse(movie[0])+1;
                            }

                        }
                        sr.Close();
                        if(!exists){
                            Console.WriteLine("Enter the movie's genres(if it has several, split them with a \"|\". If it has no genres, put nothing in)"); 
                            string genres = Console.ReadLine();
                            if(genres==null||genres==""||genres==" "){
                                genres="(no genres listed)";
                            }
                            StreamWriter sw = new StreamWriter(movies, true);
                            sw.WriteLine("{0},\"{1}\",{2}", ID, name, genres);
                            sw.Close();
                        }
                        Console.WriteLine("Enter a new movie (Y/N)?");
                        string answer = Console.ReadLine().ToUpper();
                        if (answer != "Y") { break; }
                    }
                }
            }while (choice == "1" || choice == "2");
        }
    }
}
