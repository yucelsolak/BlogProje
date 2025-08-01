using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogProje.Views.Shared.Components
{
    public class Last10ViewComponent:ViewComponent
    {
       private readonly IBlogService _blogManager;

        public Last10ViewComponent(IBlogService blogManager)
        {
            _blogManager = blogManager;
        }
        public IViewComponentResult Invoke()
        {
            var LastTen = _blogManager.GetBlogListDtos();
            return View(LastTen);
        }
    }
}
