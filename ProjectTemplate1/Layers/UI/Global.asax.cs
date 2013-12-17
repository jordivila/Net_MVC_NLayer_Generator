using System;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;

using $customNamespace$.Models.UserRequestModel;
using $customNamespace$.Models.UserSessionPersistence;
using $safeprojectname$.Common.AspNetApplicationServices;
using $safeprojectname$.Controllers;
using $safeprojectname$.Common.Mvc.Attributes;

namespace $safeprojectname$
{
    [Serializable]
    public class MvcApplication : HttpApplication
    {
        private static string _version = string.Empty;
        public static string Version
        {
            get
            {
                if (_version == string.Empty)
                {
                    _version = @System.Reflection.Assembly.GetAssembly(typeof($safeprojectname$.MvcApplication)).GetName().Version.ToString();
                }
                return _version;
            }
            set { }
        }

        public static IUserRequestModel<HttpContext, HttpCookieCollection> UserRequest
        {
            get
            {
                return UserRequestHelper<HttpContext, HttpCookieCollection>.CreateUserRequest() as IUserRequestModel<HttpContext, HttpCookieCollection>;
            }
        }

        public static IUserSessionModel<HttpContext, HttpSessionState> UserSession
        {
            get
            {
                return UserSessionHelper<HttpContext, HttpSessionState>.CreateUserSession() as IUserSessionModel<HttpContext, HttpSessionState>;
            }
        }

        public static string Name
        {
            get
            {
                return "$safeprojectname$";
            }
        }   

        public void Application_Start()
        {
            MvcApplication.RegisterViewEngines();
            MvcApplication.RegisterGlobalFilters(GlobalFilters.Filters);
            AreaRegistration.RegisterAllAreas();
            MvcApplication.RegisterRoutes(RouteTable.Routes);
            ControllerBuilder.Current.SetControllerFactory(new CustomControllerFactory());

            Application_InitEnterpriseLibrary();
        }

        public void Application_InitEnterpriseLibrary()
        {
            //DependencyFactory.SetUnityContainerProviderFactory(UnityContainerProvider.GetContainer(UnityContainerAvailable.Real));
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
            LogWriterFactory logWriterFactory = new LogWriterFactory();
            Logger.SetLogWriter(logWriterFactory.Create());
        }

        public void Application_End()
        {
            MvcApplication.UserRequest.Dispose();
        }

        public static RouteCollection GetRoutes()
        {
            return RouteTable.Routes;
        }

        protected void Application_BeginRequest()
        {
            // Warning !!!! Si quitamos esto no funciona la Valuidacion con DataAnnotations !!!
            Thread.CurrentThread.CurrentCulture = MvcApplication.UserRequest.UserProfile.Culture;
            Thread.CurrentThread.CurrentUICulture = MvcApplication.UserRequest.UserProfile.Culture;
        }

        protected static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new RequestLoggerActionAttribute());
            //filters.Add(new RequestContextItemsSetterAttibute());
            filters.Add(new HandleErrorAttributeExtended());
            filters.Add(new CompressFilterActionAttribute());
            filters.Add(new MembershipUpdateLastActivityActionAttribute());
            //filters.Add(new CacheFilterAttribute());  // Warning !!! all pages will be cached in case this line is uncomented
        }
        
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }
        
        protected static void RegisterViewEngines()
        {
            /* Performance tip */
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            //ViewEngines.Engines.Add(new RazorCustomizedViewEngine());
            //ViewEngines.Engines.Add(new MobileViewEngine());
            //ViewEngines.Engines.Add(new $safeprojectname$.ViewEngines_Custom.Samples.jQueryRazorViewEngine());
        }
  
        private void RegisterValueProviderFactories()
        {
            // this allow automatic Json Model Binding--> read more about JsonValueProviderFactory on msdn 
            //ValueProviderFactories.Factories.Add(new JsonValueProviderFactory()); // --> JsonValueProviderFactory became a built-in functionallity on MVC 3
        }
    }
}