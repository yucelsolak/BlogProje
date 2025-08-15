using Business.Constants;
using Castle.DynamicProxy;
using Core.Extensions;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Business.BusinessAspects.Autofac
{
    public class SecuredOperation : MethodInterception
    {
        private readonly string[] _roles;
        private IHttpContextAccessor _httpContextAccessor;

        public SecuredOperation(string roles)
        {
            // "Admin, Editor" gibi yazımları da kaldırmak için trim'le
            _roles = roles.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            // DI'a burada dokunma! (lazy resolve yapacağız)
        }

        protected override void OnBefore(IInvocation invocation)
        {
            // Lazy resolve: ilk çalıştığı anda al
            _httpContextAccessor ??= ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();

            var user = _httpContextAccessor?.HttpContext?.User;
            if (user is null || user.Identity is null || !user.Identity.IsAuthenticated)
                throw new SecurityException(Messages.AuthorizationDenied);

            var roleClaims = user.ClaimRoles(); // Core.Extensions içindeki helper
            // Kullanıcının rolleri, istenen rollerden herhangi birini içeriyor mu?
            var authorized = _roles.Any(required => roleClaims.Contains(required));

            if (!authorized)
                throw new SecurityException(Messages.AuthorizationDenied);
        }
    }
}