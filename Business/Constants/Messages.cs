using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public static class Messages
    {
        public static string CategoryAdded = "Kategori eklendi.";
        public static string CategoryUpdated = "Kategori Güncellendi.";
        public static string CategoryNotAllowEmpty = "Kategori Adı Boş Geçilemez.";
        public static string CategorySameName = "Aynı İsimde Bir Kategori Mevcut";
        public static string CategoryDeleted = "Kategori Silindi.";
        public static string CategoryHasBlogs = "Bu Kategoriye Ait Bloglar Mevcut Olduğu İçin Silinemez.";

        public static string BlogDeleted = "Blog Silindi.";
        public static string BlogNotAllowEmpty = "Blog Başlığı Boş Geçilemez.";
        public static string BlogDescNotAllowEmpty = "Blog İçeriği Boş Geçilemez.";
        public static string BlogCategoryNotAllowEmpty = "Bir Kategori Seçmelisiniz.";
        public static string BlogAdded = "Blog eklendi.";
        public static string BlogUpdated = "Blog güncellendi.";
    }
}
