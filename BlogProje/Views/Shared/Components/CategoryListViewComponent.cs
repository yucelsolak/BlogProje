using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogProje.Views.Shared.Components
{
    public class CategoryListViewComponent: ViewComponent
    {
        private readonly IBlogCategoryService _blogCategoryManager;

        public CategoryListViewComponent(IBlogCategoryService blogCategoryManager)
        {
            _blogCategoryManager = blogCategoryManager;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _blogCategoryManager.GetCategoriesWithBlogCount();
            return View(categories); // Default.cshtml dosyasını döndürecek
        }
    }
}
