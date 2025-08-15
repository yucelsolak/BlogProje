using Core.Entities.Concrete;
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
    public DbSet<User> Users { get; set; }
    public DbSet<Keyword> Keywords { get; set; }
    public DbSet<KeywordBlog> KeywordBlogs { get; set; }
    public DbSet<OperationClaim> OperationClaims { get; set; }
    public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User tablosu
        modelBuilder.Entity<User>(b =>
        {
            b.ToTable("Users");          // Yeni tablo adı
            b.HasKey(x => x.UserId);     // Primary Key
                                         // Kolon adların birebir property isimleriyle aynıysa ekstra mapping gerekmez
        });

        // OperationClaim tablosu
        modelBuilder.Entity<OperationClaim>(b =>
        {
            b.ToTable("OperationClaims");
            b.HasKey(x => x.Id);
        });

        // UserOperationClaim tablosu
        modelBuilder.Entity<UserOperationClaim>(b =>
        {
            b.ToTable("UserOperationClaims");
            b.HasKey(x => x.id); // id property'si küçük i değil büyük I ile Id olmalı

            b.HasOne<User>()
             .WithMany()
             .HasForeignKey(x => x.UserId) // AdminId yerine UserId
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne<OperationClaim>()
             .WithMany()
             .HasForeignKey(x => x.OperationClaimId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasIndex(x => new { x.UserId, x.OperationClaimId }).IsUnique();
        });

        modelBuilder.Entity<KeywordBlog>(b =>
        {
            b.HasKey(kb => kb.KeywordBlogId);

            b.HasOne(kb => kb.Keyword)
 .WithMany(k => k.KeywordBlogs)
 .HasForeignKey(kb => kb.KeywordId)
 .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(kb => kb.Blog)
             .WithMany(b => b.KeywordBlogs)
             .HasForeignKey(kb => kb.BlogId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasIndex(kb => new { kb.BlogId, kb.KeywordId })
             .IsUnique(); // <-- aynı çiftten bir tane olsun
        });

        base.OnModelCreating(modelBuilder);


    }
}
