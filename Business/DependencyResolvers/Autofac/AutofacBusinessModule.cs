using Autofac;
using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BlogManager>().As<IBlogService>();
            builder.RegisterType<BlogCategoryManager>().As<IBlogCategoryService>();
            //builder.RegisterType<EfBlogCategoryDal>().As<IBlogCategoryDal>();
            //builder.RegisterType<EfBlogDal>().As<IBlogDal>();

            builder.RegisterAssemblyTypes(typeof(EfBlogCategoryDal).Assembly)
    .Where(t => t.Name.StartsWith("Ef"))
    .AsImplementedInterfaces();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            //builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
            //    .EnableInterfaceInterceptors(new ProxyGenerationOptions()
            //    {
            //        Selector = new AspectInterceptorSelector()
            //    }).SingleInstance();
        }
    }
}
