using AutoMapper;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Entities;
using Core.Extensions;
using Core.Infrastructure.Abstract;
using Core.Utilities.Results;
using Core.Utilities.Security;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs.Blog;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;


namespace Business.Concrete
{
    public class BlogManager : IBlogService
    {
        IBlogDal _blogDal;
        IMapper _mapper;
        ISlugDal _slugDal;
        ISlugService _slugService;
        IImageStorage _imageStorage;
        IKeywordService _keywordService;
        IKeywordBlogDal _keywordBlogDal;
        IKeywordDal _keywordDal;
        public BlogManager(IBlogDal blogDal, IMapper mapper, ISlugDal slugDal, ISlugService slugService, IImageStorage imageStorage, IKeywordService keywordService, IKeywordBlogDal keywordBlogDal, IKeywordDal keywordDal)
        {
            _blogDal = blogDal;
            _mapper = mapper;
            _slugDal = slugDal;
            _slugService = slugService;
            _imageStorage = imageStorage;
            _keywordService = keywordService;
            _keywordBlogDal = keywordBlogDal;
            _keywordDal = keywordDal;
        }

        public List<BlogListDto> GetByCategory(int CategoryId)
        {
            return _blogDal.GetByCategory(CategoryId);
        }

        public List<BlogListDto> GetLastTenBlog()
        {
            return _blogDal.GetLastTenBlog();
        }
 
        [SecuredOperation(Permissions.AdminOnly)]
        public IResult TDelete(Blog entity)
        {

            // 0) Bu bloga bağlı keyword id'lerini, blogu silmeden ÖNCE al (DAL)
            var candidateKeywordIds = _keywordBlogDal.GetKeywordIdsByBlogId(entity.BlogId);

            // 1) Blog slug'ını sil
            _slugService.DeleteByEntity("Blog", entity.BlogId);

            // 2) Blog'u sil (FK Cascade -> KeywordBlogs düşer)
            _blogDal.Delete(entity);

            var orphanCandidateIds = candidateKeywordIds;
            _keywordDal.DeleteIfUnusedByIds(orphanCandidateIds);
            _slugDal.DeleteKeywordSlugsIfNoKeywordByIds(orphanCandidateIds);

            // 5) Görsel temizliği
            if (!string.IsNullOrWhiteSpace(entity.Image))
                _imageStorage.DeleteBlogImages(entity.Image);

            return new SuccessResult(Messages.BlogDeleted);
        }
        public Blog TGetByID(int id)
        {
            return _blogDal.Get(p => p.BlogId == id);
        }
        public List<Blog> GetAllBlog()
        {
            return _blogDal.GetAll();
        }
        List<BlogListDto> IBlogService.GetAllBlog()
        {
            return _blogDal.GetAllBlog();
        }
        public List<Blog> TGetList()
        {
            return _blogDal.GetAll();
        }
  
        public Blog GetBlogDetail(string slug)
        {
            return _blogDal.GetBlogDetail(slug);
        }

        public void IncrementViewCount(int BlogId)
        {
            _blogDal.IncrementViewCount(BlogId);
        }

        public List<BlogListDto> GetMostRead()
        {
            return _blogDal.GetMostRead();
        }
        [SecuredOperation(Permissions.BlogCreate)]
        [ValidationAspect(typeof(BlogValidator))]
        public IResult AddBlog(AddUpdateBlogDto dto)
        {
            var entity=_mapper.Map<Blog>(dto);
            var image = entity.Title.ToSlug()+".jpg";
            entity.AddedTime = DateTime.UtcNow;
            entity.Image = image;
            _blogDal.Add(entity);
            var names = ParseKeywords(dto.Keywords);               // private helper (BlogManager)
            var keywordIds = _keywordService.UpsertAndGetIds(names); // <-- servis çağrısı
            SyncBlogKeywords(entity.BlogId, keywordIds);             // <-- ilişkiyi kur

            _slugService.AddSlug(dto.Title, "Blog", entity.BlogId);
            return new SuccessResult(Messages.BlogAdded);
        }

        public async Task<string> SaveBlogImage(IFormFile blogImage, string title)
        {
            var fileName = title.ToSlug() + ".jpg";  // Dosya adı her zaman .jpg olarak oluşturulacak

            if (blogImage != null && blogImage.Length > 0)
            {
                var bigPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/BlogImages/big", fileName);
                var smallPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/BlogImages/small", fileName);

                using (var stream = blogImage.OpenReadStream())
                using (var image = SixLabors.ImageSharp.Image.Load(stream)) // ImageSharp kütüphanesi ile resmi yüklüyoruz
                {
                    // 900px genişlik için büyük görsel
                    var bigImage = image.Clone(ctx => ctx.Resize(900, 0));
                    await using var bigStream = new FileStream(bigPath, FileMode.Create);
                    await bigImage.SaveAsJpegAsync(bigStream); // JPEG olarak kaydediyoruz

                    // 300px genişlik için küçük görsel
                    var smallImage = image.Clone(ctx => ctx.Resize(300, 0));
                    await using var smallStream = new FileStream(smallPath, FileMode.Create);
                    await smallImage.SaveAsJpegAsync(smallStream); // JPEG olarak kaydediyoruz
                }

                return fileName; // Resmin kaydedilen adını döndürüyoruz
            }

            return null; // Eğer resim yoksa null döneriz
        }
        
        public async Task UpdateBlogImages(int blogId, IFormFile newImage, string slug)
        {
            var blog = _blogDal.Get(p => p.BlogId == blogId);

            // Eski resmi silme işlemi
            if (newImage != null && newImage.Length > 0)
            {
                // Eski resmi silme işlemi
                if (!string.IsNullOrEmpty(blog.Image))
                {
                    var smallImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/BlogImages/small", blog.Image);
                    var bigImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/BlogImages/big", blog.Image);

                    // Small resim var mı, sil
                    if (File.Exists(smallImagePath))
                    {
                        File.Delete(smallImagePath);
                    }

                    // Big resim var mı, sil
                    if (File.Exists(bigImagePath))
                    {
                        File.Delete(bigImagePath);
                    }
                }
                // Yeni resmi kaydetme
                var fileName = await SaveBlogImage(newImage, slug.ToSlug()); // Yeni resmi kaydediyoruz

            }
        }

        public List<BlogListDto> GetAdmin50Blog()
        {
            return _blogDal.GetAdmin50Blog();
        }

        public List<BlogListDto> SearchBlogAdmin(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return _blogDal.GetAdmin50Blog();
            }

            searchTerm = searchTerm.Trim().ToLower();

            var filtered = _blogDal.GetAllWithCategory(
                                p => (p.Title ?? "").ToLower().Contains(searchTerm)
                           ) ?? new List<Blog>();

            return filtered.Select(b => new BlogListDto
            {
                BlogId = b?.BlogId ?? 0,
                Title = b?.Title ?? "",
                CategoryName = b?.Category?.CategoryName ?? "",
                Status = b?.Status ?? false
            }).ToList();
        }
        [SecuredOperation(Permissions.AdminOnly)]
        [ValidationAspect(typeof(BlogValidator))]
        [TransactionScopeAspect] // bu metod içindeki tüm db işlemlerini ya yap ya da hiç birini yapma
        public async Task<IResult> BlogWithSlugUpdate(AddUpdateBlogDto dto)
        {
            var blog = _blogDal.Get(p => p.BlogId == dto.BlogId);
            if (blog == null) return new ErrorResult();

            // --- 1) Blog alanları ---
            blog.Title = dto.Title;
            blog.Description = dto.Description;
            blog.Status = dto.Status;
            blog.CategoryId = dto.CategoryId;

            // --- 2) Keywords: parse -> upsert -> sync ---
            var names = ParseKeywords(dto.Keywords); // dto.Keywords: "asp.net, c#, ef core" gibi
            var keywordIds = _keywordService.UpsertAndGetIds(names); // Keyword tablosunda varsa al, yoksa ekle
            var sync = SyncBlogKeywords(blog.BlogId, keywordIds);
            var removedIds = sync.Removed;               // Köprü tabloyu senkronize et

            _keywordDal.DeleteIfUnusedByIds(removedIds);
            _slugDal.DeleteKeywordSlugsIfNoKeywordByIds(removedIds);

            // --- 3) Slug ---
            var slug = _slugService.GetByEntity("Blog", dto.BlogId);
            if (slug != null) _slugService.UpdateSlug(slug.SlugId, dto.Title);

            // --- 4) Görsel ---
            if (dto.BlogImage != null && dto.BlogImage.Length > 0)
            {
                await UpdateBlogImages(dto.BlogId, dto.BlogImage, dto.Title);
                blog.Image = dto.Title.ToSlug() + ".jpg";
            }

            _blogDal.Update(blog);
            return new SuccessResult(Messages.BlogUpdated);
        }
        private static List<string> ParseKeywords(string? csv)
        {
            return (csv ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(k => k.Trim())
                .Where(k => !string.IsNullOrWhiteSpace(k))
                .Distinct(StringComparer.InvariantCultureIgnoreCase)
                .ToList();
        }
        private (List<int> Added, List<int> Removed) SyncBlogKeywords(int blogId, List<int> desiredKeywordIds)
        {
            var currentIds = _keywordBlogDal
        .GetAll(kb => kb.BlogId == blogId)
        .Select(kb => kb.KeywordId)
        .ToList();

            var toAdd = desiredKeywordIds.Except(currentIds).ToList();
            var toRemove = currentIds.Except(desiredKeywordIds).ToList();

            if (toAdd.Count > 0)
                foreach (var kid in toAdd)
                    _keywordBlogDal.Add(new KeywordBlog { BlogId = blogId, KeywordId = kid });

            if (toRemove.Count > 0)
            {
                var rows = _keywordBlogDal.GetAll(kb => kb.BlogId == blogId && toRemove.Contains(kb.KeywordId));
                foreach (var row in rows)
                    _keywordBlogDal.Delete(row);
            }

            return (toAdd, toRemove);
        }

        public List<Blog> GetByKeywordId(int keywordId)
        {
            return _blogDal.GetBlogsByKeywordId(keywordId);
        }

        public List<BlogListDto> GetBlogListByKeywordId(int keywordId)
        {
            return _blogDal.GetBlogListByKeywordId(keywordId);
        }
    }
}
