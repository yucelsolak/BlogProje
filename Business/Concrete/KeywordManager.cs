using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
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
        public KeywordManager(IKeywordDal keywordDal, ISlugService slugService)
        {
            _keywordDal = keywordDal;
            _slugService = slugService;
        }
        public IResult TDelete(Keyword entity)
        {
            throw new NotImplementedException();
        }

        public Keyword TGetByID(int id)
        {
            throw new NotImplementedException();
        }

        public List<Keyword> TGetList()
        {
            throw new NotImplementedException();
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
    }
}
