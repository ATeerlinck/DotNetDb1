using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog.Web;

namespace module9lab
{
    public class MovieFile
    {
        // public property
        public string filePath { get; set; }
        public List<Movie> Movies { get; set; }
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();

        // constructor is a special method that is invoked
        // when an instance of a class is created
        public MovieFile(string movieFilePath)
        {
            filePath = movieFilePath;
            Movies = new List<Movie>();

            // to populate the list with data, read from the data file
            try
            {
                StreamReader sr = new StreamReader(filePath);
                
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
                        if(movie.Length-4!=1){
                            for(int i=2;i<movie.Length-2;i++){
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
                        Movie mov = new Movie();
                        int length = movie.Length;
                        string[] genres = movie[length-3].Split("|");
                        for(int i = 0; i<genres.Length; i++){
                            mov.genres.Add(genres[i]);
                        }
                        mov.mediaId = UInt64.Parse(movie[0]);
                        mov.title = movie[1];
                        mov.director = movie[length-2];
                        mov.runningTime = TimeSpan.Parse(movie[length-1]);
                        Movies.Add(mov);
                    }      
                }
                // close file when done
                sr.Close();
                logger.Info("Movies in file {Count}", Movies.Count);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        // public method
        public bool isUniqueTitle(string title)
        {
            if (Movies.ConvertAll(m => m.title.ToLower()).Contains(title.ToLower()))
            {
                logger.Info("Duplicate movie title {Title}", title);
                return false;
            }
            return true;
        }

        public void AddMovie(Movie movie)
        {
            try
            {
                // first generate movie id
                movie.mediaId = Movies.Max(m => m.mediaId) + 1;
                StreamWriter sw = new StreamWriter(filePath, true);
                sw.WriteLine($"{movie.mediaId},{movie.title},{string.Join("|", movie.genres)},{movie.director},{movie.runningTime}");
                sw.Close();
                // add movie details to Lists
                Movies.Add(movie);
                // log transaction
                logger.Info("Movie id {Id} added", movie.mediaId);
            } 
            catch(Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
}
