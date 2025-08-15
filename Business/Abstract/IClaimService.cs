using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.DTOs.Admin;
using Entities.DTOs.Claim;
using Entities.DTOs.UserClaim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IClaimService:IGenericService<OperationClaim>
    {
        IResult AddClaim(AddUpdateClaim dto);
        IResult UpdateClaim(AddUpdateClaim dto);
        List<AddUpdateUserClaim> GetClaims(int userId);

        IResult AddUserClaims(int userId, IEnumerable<int> claimIds);
        IResult RemoveUserClaims(int userId, IEnumerable<int> claimIds);
        List<string> GetRoleNamesByUserId(int userId);
    }
}
