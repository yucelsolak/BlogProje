using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Abstract
{
    public interface IImageStorage
    {
        void DeleteBlogImages(string fileName);
    }
}
