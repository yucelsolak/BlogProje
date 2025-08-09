using AutoMapper;
using Entities.Concrete;
using Entities.DTOs.Blog;
using Entities.DTOs.Category;
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


            // Blog -> BlogDetailDto
            CreateMap<Blog, BlogDetailDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));

            CreateMap<AddCategoryDto, BlogCategory>();
            CreateMap<UpdateCategoryDto, BlogCategory>();
            CreateMap<AddUpdateBlogDto,Blog > ();
        }
    }
}
