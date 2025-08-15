using Core.DataAccess;
using Core.Entities.Concrete;
using Entities.DTOs.UserClaim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IUserClaimDal:IEntityRepository<UserOperationClaim>
    {
        List<AddUpdateUserClaim> GetByClaim(int OperationClaimId);
    }
}
