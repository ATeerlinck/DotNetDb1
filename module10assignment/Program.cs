using System;
using NLog.Web;
using System.IO;
using System.Linq;

namespace module10assignment
{
    class Program
    {
        // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");
            string choice;
            do{
                Console.WriteLine("1) Display all blogs");
                Console.WriteLine("2) Add Blog.");
                Console.WriteLine("3) Create Post.");
                Console.WriteLine("4) Display Posts.");
                Console.WriteLine("Enter any other key to exit.");
                choice = Console.ReadLine();

                if(choice == "1"){
                    try
                    {   
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.Name);

                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                    }
                }
                if(choice == "2"){
                    Console.Write("Enter a name for a new Blog: ");
                    try{
                        var name = Console.ReadLine();

                        var blog = new Blog { Name = name };

                        var db = new BloggingContext();
                        db.AddBlog(blog);
                        logger.Info("Blog added - {name}", name);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                    }
                }
                if(choice == "3"){
                    try{
                        Console.Write("Please select a blog:\n");
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.Name);
                        int count = 1;
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{count}) {item.Name}");
                        }
                        var array = query.ToArray();
                        var blog = array[ Convert.ToInt32(Console.ReadLine())-1];
                        Post post = new Post();
                        post.BlogId = blog.BlogId;
                        post.Blog = blog;
                        Console.WriteLine("\nWhat is the title of the post?");
                        post.Title = Console.ReadLine();
                        Console.WriteLine("\nWhat is the content of the post?");
                        post.Content = Console.ReadLine();
                        db.AddPost(post);
                        logger.Info("Post added - {0}", post.Title); 
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                    }
                }
                
            }while(choice == "1"||choice == "2"||choice == "3"||choice == "4");
            logger.Info("Program ended");
        }
    }
}
