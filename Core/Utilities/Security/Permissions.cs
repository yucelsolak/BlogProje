using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security
{
    public static class Permissions
    {
        public const string AdminOnly = "Admin";
        public const string BlogCreate = "Editor,Admin";
        public const string AdminOrEditor = "Admin,Editor"; // UI panel kapısı için
    }
}
