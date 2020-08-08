using Autofac;
using Grand.Core.Configuration;
using Grand.Core.Infrastructure;
using Grand.Core.Infrastructure.DependencyManagement;
using Grand.Plugin.Widgets.AjaxFilters.Controllers;
using Grand.Plugin.Widgets.AjaxFilters.Services;

namespace Grand.Plugin.Widgets.AjaxFilters.Infrastructure
{
    public partial class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, GrandConfig config)
        {
            builder.RegisterType<AjaxFiltersPlugin>().InstancePerLifetimeScope();
            builder.RegisterType<AjaxFiltersService>().As<IAjaxFiltersService>().InstancePerLifetimeScope();
        }

        public void AfterRegister(ContainerBuilder builder, ITypeFinder typeFinder, GrandConfig config)
        {
            builder.RegisterType<AjaxCatalogController>().PropertiesAutowired();
        }

        public int Order => 1000;
    }
}
