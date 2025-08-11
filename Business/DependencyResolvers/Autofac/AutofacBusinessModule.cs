using Autofac;
using AutoMapper;
using Business.Abstract;
using Business.Concrete;
using Business.Mapping;
using Core.Infrastructure.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BlogManager>().As<IBlogService>();
            builder.RegisterType<BlogCategoryManager>().As<IBlogCategoryService>();
            builder.RegisterType<SlugManager>().As<ISlugService>();
            builder.RegisterAssemblyTypes(typeof(EfBlogCategoryDal).Assembly)
                .Where(t => t.Name.StartsWith("Ef"))
                .AsImplementedInterfaces();

            // ✅ Tüm AutoMapper profillerini tara
            builder.Register(c => new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(MappingProfile).Assembly); // 👈 kritik satır
            })).AsSelf().SingleInstance();

            builder.Register(c =>
            {
                var context = c.Resolve<ILifetimeScope>();
                var config = c.Resolve<MapperConfiguration>();
                return config.CreateMapper(context.Resolve);
            }).As<IMapper>().InstancePerLifetimeScope();



        }
    }
}
