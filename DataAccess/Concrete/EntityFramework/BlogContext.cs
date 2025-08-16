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
 .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(kb => kb.Blog)
             .WithMany(b => b.KeywordBlogs)
             .HasForeignKey(kb => kb.BlogId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasIndex(kb => new { kb.BlogId, kb.KeywordId })
             .IsUnique(); // <-- aynı çiftten bir tane olsun
        });

        modelBuilder.Entity<PhotoGallery>(b =>
        {
            b.HasKey(x => x.PhotoGalleryId);

            // Blog silinirse galeri bağımsız kalsın istiyorsan:
            b.HasOne(x => x.Blog)
             .WithMany(b => b.PhotoGalleries)
             .HasForeignKey(x => x.BlogId)
             .OnDelete(DeleteBehavior.SetNull); // BlogId nullable olmalı

            // Aynı Blog içinde başlıklar benzersiz olsun (opsiyonel):
            b.HasIndex(x => new { x.BlogId, x.Title }).IsUnique();
        });

        modelBuilder.Entity<GalleryImage>(b =>
        {
            b.HasKey(x => x.GalleryImageId);

            b.HasOne(x => x.PhotoGallery)
             .WithMany(g => g.Images)
             .HasForeignKey(x => x.PhotoGalleryId)
             .OnDelete(DeleteBehavior.Cascade); // Galeri silinince resimler de silinsin

            b.HasIndex(x => new { x.PhotoGalleryId, x.SortOrder });
        });

        base.OnModelCreating(modelBuilder);


    }
}
