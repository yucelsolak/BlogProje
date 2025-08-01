using AutoMapper;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            // Blog -> BlogListDto
            CreateMap<Blog, BlogListDto>()
    .ForMember(d => d.ShortDescription, opt => opt.MapFrom(s =>
        string.IsNullOrEmpty(s.Description)
            ? string.Empty
            : (s.Description.Length > 200 ? s.Description.Substring(0, 200) + "..." : s.Description)));

            // Blog -> BlogDetailDto
            CreateMap<Blog, BlogDetailDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));

            // BlogCategory -> CategoryWithBlogCountDto
            CreateMap<BlogCategory, CategoryWithBlogCountDto>()
                .ForMember(dest => dest.BlogCount, opt => opt.MapFrom(src => src.Blogs.Count));
        }
    }
}
