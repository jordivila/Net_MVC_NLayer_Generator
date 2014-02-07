using System.IO;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using $customNamespace$.Models.Globalization;
using $customNamespace$.Models.UserRequestModel;
using $customNamespace$.Models.Unity;
using $customNamespace$.UI.Web.Unity;

namespace $safeprojectname$.Proxies
{
    [TestClass]
    public abstract class TestProxyBase: TestBase
    {
        //private CulturesAvailable currentCulture = MvcApplication30.Models.Enumerations.CulturesAvailable.es;
        //private string currentCulture = "es-ES";
        //public static string currentCultureName = TestCommon.CultureDefault;


        public virtual void MyTestInitialize()
        {
            
        }



    }
}