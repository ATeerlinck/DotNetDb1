using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace module10assignment
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        public void AddBlog(Blog blog)
        {
            this.Blogs.Add(blog);
            this.SaveChanges();
        }
        public void AddPost(Post post){
            this.Posts.Add(post);
            this.SaveChanges();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            optionsBuilder.UseSqlServer(@config["BloggingContext:ConnectionString"]);
        }
    }
}
