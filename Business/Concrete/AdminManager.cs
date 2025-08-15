using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security;
using Core.Utilities.Security.Hasing;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    internal class AdminManager:IAdminService
    {
        IAdminDal _adminDal;
        public AdminManager(IAdminDal adminDal)
        {
            _adminDal = adminDal;
        }
        [SecuredOperation(Permissions.AdminOnly)]
        [ValidationAspect(typeof(AdminValidator))]
        public IResult AddAdmin(AddUpdateAdmin dto)
        {
            HashingHelper.CreatePasswordHash(dto.Password, out var hash, out var salt);

            var admin = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                Status = true
            };

            _adminDal.Add(admin);
            return new SuccessResult(Messages.AdminAdded);
        }

        [SecuredOperation(Permissions.AdminOnly)]
        public IResult TDelete(User entity)
        {
            _adminDal.Delete(entity);
            return new SuccessResult(Messages.AdminDeleted);
        }
        [SecuredOperation(Permissions.AdminOnly)]
        public User TGetByID(int id)
        {
           return _adminDal.Get(x=>x.UserId==id);
        }
        [SecuredOperation(Permissions.AdminOnly)]
        public List<User> TGetList()
        {
            return _adminDal.GetAll().OrderByDescending(x => x.UserId)
                .ToList();
        }
        [SecuredOperation(Permissions.AdminOnly)]
        [ValidationAspect(typeof(AdminValidator))]
        public IResult UpdateAdmin(AddUpdateAdmin dto)
        {
            var admin = _adminDal.Get(a => a.UserId == dto.UserId);
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                HashingHelper.CreatePasswordHash(dto.Password, out var hash, out var salt);
                admin.PasswordHash = hash;
                admin.PasswordSalt = salt;
            }
            admin.Name = dto.Name;
            admin.Email = dto.Email;
            admin.Status = true;

            _adminDal.Update(admin);
            return new SuccessResult(Messages.AdminUpdated);
        }

    }
}
