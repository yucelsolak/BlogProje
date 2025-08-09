using AutoMapper;
using Business.Abstract;
using Core.Entities;
using Core.Extensions;
using Core.Infrastructure.Abstract;
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
        public BlogManager(IBlogDal blogDal, IMapper mapper, ISlugDal slugDal, ISlugService slugService,IImageStorage imageStorage)
        {
            _blogDal = blogDal;
            _mapper = mapper;
            _slugDal = slugDal;
            _slugService = slugService;
            _imageStorage=imageStorage;
        }

        public List<BlogListDto> GetByCategory(int CategoryId)
        {
            return _blogDal.GetByCategory(CategoryId);
        }

         public List<BlogListDto> GetLastTenBlog()
        {
            return _blogDal.GetLastTenBlog();
        }

        public void TAdd(Blog entity)
        {
            _blogDal.Add(entity);
        }

        public void TDelete(Blog entity)
        {
            var fileName = entity.Image; // dosya adını sakla
            _slugService.DeleteByEntity("Blog", entity.BlogId);
            _blogDal.Delete(entity);

            if (!string.IsNullOrWhiteSpace(fileName))
                _imageStorage.DeleteBlogImages(fileName);
        }

        public Blog TGetByID(int id)
        {
           return _blogDal.Get(p => p.BlogId == id);
        }

        public List<Blog> GetAllBlog()
        {
            return _blogDal.GetAll();
        }

        public void TUpdate(Blog entity)
        {
            _blogDal.Update(entity);
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



        public void AddBlog(AddUpdateBlogDto dto)
        {
            Console.WriteLine("DTO Tipi: " + dto.GetType().FullName);
            var entity=_mapper.Map<Blog>(dto);
            var image = entity.Title.ToSlug()+".jpg";
            entity.AddedTime = DateTime.UtcNow;
            entity.Image = image;
            _blogDal.Add(entity);

            var slugText=entity.Title.ToSlug();

            var slug = new Slug
            {
                SlugText = slugText,
                EntityId = entity.BlogId,
                EntityType = "Blog"
            };
            _slugDal.Add(slug);
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

        public async Task BlogWithSlugUpdate(Blog entity, string slugText, IFormFile newImage)
        {
            var blog = _blogDal.Get(p => p.BlogId == entity.BlogId);
          

            var slug = _slugService.GetByEntity("Blog", entity.BlogId);
            if (slug != null)
            {
                slug.SlugText = slugText.ToSlug();
                _slugDal.Update(slug);
            }
            // Resim güncelleme işlemi
            if ( newImage != null && newImage.Length > 0)
            {
                // Eski resimleri silme ve yeni resmi kaydetme işlemi
               await UpdateBlogImages(entity.BlogId, newImage, slugText);
            entity.Image = slugText.ToSlug()+".jpg";
            _blogDal.Update(entity);
            }
            else
            {
                entity.Image = blog.Image;
                _blogDal.Update(entity);
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
    }
}
