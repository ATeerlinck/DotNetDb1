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
                            count++;
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
                if(choice == "4"){
                    try
                    {   
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.Name);
                        int count = 1;
                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{count}) {item.Name}");
                            count++;
                        }
                        var array = query.ToArray();
                        var blog = array[ Convert.ToInt32(Console.ReadLine())-1];
                        var posts = db.Posts.Where(b => b.BlogId.Equals(blog.BlogId));
                        foreach (var item in posts)
                        {
                            Console.Write("\nTitle: ");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine(item.Title);
                            Console.ResetColor();
                            Console.Write("Content: ");
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine(item.Content);
                            Console.ResetColor();
                            Console.Write("This post was found within the ");
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.Write(blog.Name); //I personally think putting the blog name for each post instead of once at the end of the post is redundant. I will leave it here for now, but I do think this getting removed would be fantastic.
                            Console.ResetColor();
                            Console.WriteLine(" blog.\n");
                        }
                        Console.WriteLine($"\nThere were {count} posts in {blog.Name} blog");
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
