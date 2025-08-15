using Core.DataAccess.Concrete;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using Entities.DTOs.UserClaim;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    internal class EfUserClaimDal : EfEntityRepositoryBase<UserOperationClaim, BlogContext>, IUserClaimDal
    {
        public List<AddUpdateUserClaim> GetByClaim(int OperationClaimId)
        {
            using var context = new BlogContext();
            var result=(from userClaim in context.UserOperationClaims
                        where userClaim.OperationClaimId==OperationClaimId
                        select new
                        {
                            userClaim.UserId,
                            userClaim.OperationClaimId,
                            userClaim.id
                        })
                        .AsNoTracking()
                        .ToList();
            return result.Select(b=>new AddUpdateUserClaim { 
            UserId=b.UserId,
            OperationClaimId=b.OperationClaimId,    
            id=b.id
            }).ToList();
        }
    }
}
