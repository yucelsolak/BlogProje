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
    public class EfClaimDal : EfEntityRepositoryBase<OperationClaim, BlogContext>, IClaimDal
    {
        public List<AddUpdateUserClaim> GetClaims(int userId)
        {
            using var context = new BlogContext();
            var result = (from userClaim in context.UserOperationClaims
                          where userClaim.UserId == userId
                          select new
                          {
                              userClaim.id,
                              userClaim.UserId,
                              userClaim.OperationClaimId
                          })
                        .AsNoTracking()
                        .ToList();
            return result.Select(b => new AddUpdateUserClaim
            {
                UserId = b.UserId,
                OperationClaimId = b.OperationClaimId,
                id = b.id
            }).ToList();
        }

        public List<string> GetRoleNamesByUserId(int userId)
        {
            using var context = new BlogContext();
            return (from uoc in context.UserOperationClaims.AsNoTracking()
                    join oc in context.OperationClaims.AsNoTracking()
                         on uoc.OperationClaimId equals oc.Id
                    where uoc.UserId == userId
                    select oc.Name)
                   .ToList();
        }
    }
}
