using AutoMapper;
using Core.Entities.Concrete;
using Entities.Concrete;
using Entities.DTOs.Admin;
using Entities.DTOs.Blog;
using Entities.DTOs.Category;
using Entities.DTOs.Claim;
using Entities.DTOs.UserClaim;
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
            CreateMap<AddUpdateAdmin, User>();
            CreateMap<AddUpdateClaim, OperationClaim>();
            CreateMap<AddUpdateUserClaim, UserOperationClaim>();
            CreateMap<OperationClaim, AddUpdateClaim>();
            CreateMap<Keyword, KeywordLinkDto>();
    //            .ForMember(d => d.KeywordId, m => m.MapFrom(s => s.KeywordId))
    //.ForMember(d => d.KeywordName, m => m.MapFrom(s => s.KeywordName))
    //.ForMember(d => d.Slug, m => m.Ignore());
            CreateMap<KeywordLinkDto, Keyword>();


        }
    }
}
