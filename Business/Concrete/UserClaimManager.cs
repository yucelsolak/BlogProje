using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    internal class UserClaimManager : IUserClaimService
    {

        public IResult TDelete(UserOperationClaim entity)
        {
            throw new NotImplementedException();
        }

        public UserOperationClaim TGetByID(int id)
        {
            throw new NotImplementedException();
        }

        public List<UserOperationClaim> TGetList()
        {
            throw new NotImplementedException();
        }

    }
}
