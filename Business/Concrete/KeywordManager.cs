using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Core.Entities;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class KeywordManager : IKeywordService
    {
        IKeywordDal _keywordDal;
        ISlugService _slugService;
        IMapper _mapper;
        IKeywordBlogDal _keywordBlogDal;
        public KeywordManager(IKeywordDal keywordDal, ISlugService slugService,IMapper mapper, IKeywordBlogDal keywordBlogDal)
        {
            _keywordDal = keywordDal;
            _slugService = slugService;
            _mapper = mapper;
            _keywordBlogDal = keywordBlogDal;
        }
        public IResult TDelete(Keyword entity)
        {
            _slugService.DeleteByEntity("Keyword", entity.KeywordId);
            _keywordDal.Delete(entity);            
            return new SuccessResult(Messages.KeywordDeleted);
        }

        public Keyword TGetByID(int id)
        {
            return _keywordDal.Get(p=>p.KeywordId == id);
        }

        public List<Keyword> TGetList()
        {
            return _keywordDal.GetAll();
        }

        public List<int> UpsertAndGetIds(IEnumerable<string> names)
        {
            var ids = new List<int>();
            foreach (var raw in names)
            {
                var name = Normalize(raw);
                if (string.IsNullOrWhiteSpace(name)) continue;

                // Case-insensitive arama
                var existing = _keywordDal.Get(k => k.KeywordName.ToLower() == name.ToLower());
                if (existing != null)
                {
                    ids.Add(existing.KeywordId);
                    continue;
                }

                var entity = new Keyword { KeywordName = name };
                _keywordDal.Add(entity);

                // Slug tekilleştirme
                _slugService.AddSlug(name, "Keyword", entity.KeywordId);

                ids.Add(entity.KeywordId);
            }
            return ids.Distinct().ToList();
        }
        private static string Normalize(string s) => s.Trim();

        public IList<string> GetNamesByBlogId(int blogId)
        {
            return _keywordDal.GetNamesByBlogId(blogId);
        }

        public string GetCsvByBlogId(int blogId)
        {
            var names = _keywordDal.GetNamesByBlogId(blogId);
            return names == null || names.Count == 0 ? string.Empty : string.Join(", ", names);
        }

        public IList<string> Suggest(string term, int limit = 10)
        {
            return _keywordDal.SuggestNames(term, limit);
        }

        public IList<KeywordLinkDto> GetLinksForBlog(int blogId)
        {
            return _keywordDal.GetLinksForBlog(blogId);
        }

        public IResult UpdateKeyword(KeywordLinkDto dto)
        {
            var normalizedName = dto.KeywordName.Trim();

            var exist=_keywordDal.GetAll(p=>p.KeywordId != dto.KeywordId 
            && p.KeywordName.ToLower()==normalizedName.ToLower()).Any();

            if (exist)
                return new ErrorResult(Messages.ExistingKeyword);

            var keyword=_mapper.Map<Keyword>(dto);
            _keywordDal.Update(keyword);
            var slug = _slugService.GetByEntity("Keyword", dto.KeywordId);
            if (slug != null)
            {
                _slugService.UpdateSlug(slug.SlugId, dto.KeywordName);
            }
            return new SuccessResult(Messages.KeywordUpdated);
        }

        public List<KeywordLinkDto> SearchKeyword(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return _mapper.Map<List<KeywordLinkDto>>(_keywordDal.GetAll());


            var term = searchTerm.Trim();
            var list = _keywordDal.GetAll(k => (k.KeywordName ?? "").Contains(term));
            return _mapper.Map<List<KeywordLinkDto>>(list);
        }
    }
}
