using Autofac;
using Grand.Core.Infrastructure;
using Grand.Core.Infrastructure.DependencyManagement;
using Grand.Plugin.Widgets.SwiperSlider.Services;
using Grand.Core.Configuration;

namespace Grand.Plugin.Widgets.SwiperSlider
{
    public partial class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get
            {
                return 1000;
            }
        }

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, GrandConfig config)
        {
            builder.RegisterType<SlideService>().As<ISlideService>().InstancePerLifetimeScope();
            builder.RegisterType<SwiperSliderPlugin>().InstancePerLifetimeScope();

            ////data context
            //builder.RegisterPluginDataContext<SlideObjectContext>("nop_object_context_slider_swiper");

            ////override required repository with our custom context
            //builder.RegisterType<EfRepository<Slide>>().As<IRepository<Slide>>()
            //    .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_slider_swiper"))
            //    .InstancePerLifetimeScope();
        }
    }
}
