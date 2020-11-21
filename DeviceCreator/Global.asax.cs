using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DeviceCreator.Models.Factory;
using DeviceManager;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

namespace DeviceCreator
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Simple injector
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            // Register classes here
            container.Register<IDevicePublisher, DevicePublisher>(Lifestyle.Singleton);
            container.Register<IDeviceFactory, DeviceFactory>(Lifestyle.Singleton);
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.RegisterMvcIntegratedFilterProvider();
            container.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}
