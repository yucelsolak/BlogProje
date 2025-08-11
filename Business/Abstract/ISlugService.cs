using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ISlugService:IGenericService<Slug>
    {
        Task<string> GetSlugAsync(string entityType, int entityId);
        Task<Dictionary<int, string>> GetSlugsByEntityTypeAsync(string entityType);
        void DeleteByEntity(string entityType, int entityId);
        Slug? GetByEntity(string entityType, int entityId);
        Slug AddSlug(string slugText, string entityType, int entityId);
        Slug UpdateSlug(int slugId, string newText);

    }
}
