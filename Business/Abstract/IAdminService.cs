using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs.Admin;
using Entities.DTOs.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAdminService:IGenericService<User>
    {
        IResult AddAdmin(AddUpdateAdmin dto);
        IResult UpdateAdmin(AddUpdateAdmin dto);
    }
}
