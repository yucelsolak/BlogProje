using Business.Abstract;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        IAdminDal _adminDal;

        public UserManager(IAdminDal userDal)
        {
            _adminDal = userDal;
        }



        public void Add(User user)
        {
            _adminDal.Add(user);
        }

        public User GetByMail(string email)
        {
            return _adminDal.Get(u => u.Email == email);
        }

        public List<OperationClaim> GetClaims(User user)
        {
            throw new NotImplementedException();
        }
    }
}