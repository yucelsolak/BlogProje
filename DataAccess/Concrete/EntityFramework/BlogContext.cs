using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

public class BlogContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=YUCEL\\SQLEXPRESS;Database=BlogProje;Trusted_Connection=True");
    }

    public DbSet<BlogCategory> BlogCategories { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Slug> Slugs { get; set; }
    public DbSet<Admin> Admins { get; set; }


}
