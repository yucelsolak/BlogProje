using Core.Utilities.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogProje.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Permissions.AdminOrEditor)] // sadece Admin veya Editor panele girebilir
    public abstract class AdminBaseController : Controller
    {
    }
}
