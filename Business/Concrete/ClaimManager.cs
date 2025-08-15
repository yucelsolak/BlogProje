using AutoMapper;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules;
using Core.Aspects.Autofac.Validation;
using Core.Entities;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.DTOs.Claim;
using Entities.DTOs.UserClaim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    internal class ClaimManager : IClaimService
    {
        IClaimDal _claimDal;
        IUserClaimDal _userClaimDal;
        IMapper _mapper;
        public ClaimManager(IClaimDal claimDal,IMapper mapper, IUserClaimDal userClaimDal)
        {
            _claimDal = claimDal;
            _mapper = mapper;
            _userClaimDal = userClaimDal;
        }
        [ValidationAspect(typeof(ClaimValidator))]
        [SecuredOperation(Permissions.AdminOnly)]
        public IResult AddClaim(AddUpdateClaim dto)
        {
            var existingClaim=_claimDal.Get(p=>p.Name.ToLower()==dto.Name.ToLower());
            if (existingClaim != null)
                return new ErrorResult(Messages.ExistingClaim);

            var claim = _mapper.Map<OperationClaim>(dto);
            _claimDal.Add(claim);
            return new SuccessResult(Messages.ClaimAdded);
        }
        [SecuredOperation(Permissions.AdminOnly)]
        public IResult AddUserClaims(int userId, IEnumerable<int> claimIds)
        {
            var list = (claimIds ?? Enumerable.Empty<int>()).Distinct().ToList();
            if (!list.Any()) return new SuccessResult();

            foreach (var cid in list)
                _userClaimDal.Add(new UserOperationClaim { UserId = userId, OperationClaimId = cid });

            return new SuccessResult("Eklendi");
        }
        [SecuredOperation(Permissions.AdminOnly)]
        public List<AddUpdateUserClaim> GetClaims(int userId)
        {
            return _claimDal.GetClaims(userId);
        }

        public List<string> GetRoleNamesByUserId(int userId)
        {
            return _claimDal.GetRoleNamesByUserId(userId) ?? new List<string>();
        }

        [SecuredOperation(Permissions.AdminOnly)]
        public IResult RemoveUserClaims(int userId, IEnumerable<int> claimIds)
        {
            var list = (claimIds ?? Enumerable.Empty<int>()).Distinct().ToList();
            if (!list.Any()) return new SuccessResult();

            var rows = _userClaimDal.GetAll(x => x.UserId == userId && list.Contains(x.OperationClaimId));
            foreach (var row in rows)
                _userClaimDal.Delete(row);

            return new SuccessResult("Silindi");
        }

        [SecuredOperation(Permissions.AdminOnly)]
        public IResult TDelete(OperationClaim entity)
        {
            var HasClaim=_userClaimDal.GetByClaim(entity.Id)?.Any()==true;
            
            if (HasClaim)
                return new ErrorResult(Messages.ClaimHasUsers);

            _claimDal.Delete(entity);
            return new SuccessResult(Messages.ClaimDeleted);
        }

        public OperationClaim TGetByID(int id)
        {
            return _claimDal.Get(p=>p.Id==id);
        }

        public List<OperationClaim> TGetList()
        {
           return _claimDal.GetAll();
        }
        [ValidationAspect(typeof(ClaimValidator))]

        public IResult UpdateClaim(AddUpdateClaim dto)
        {
            var normalizedName = dto.Name.Trim();

            var exists = _claimDal
        .GetAll(c => c.Id != dto.Id &&
                      c.Name.ToLower() == normalizedName.ToLower())
        .Any(); // <- Burada Any, List/Enumerable üstünde

            if (exists)
                return new ErrorResult(Messages.ExistingClaim);

            var claim = _mapper.Map<OperationClaim>(dto);
            _claimDal.Update(claim);
            return new SuccessResult(Messages.ClaimUpdated);
        }
    }
}
