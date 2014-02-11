using System.IO;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using $customNamespace$.Models.Globalization;
using $customNamespace$.Models.UserRequestModel;
using $customNamespace$.Models.Unity;
using $customNamespace$.UI.Web.Unity;
using $customNamespace$.Tests.Common;


namespace $customNamespace$.Tests.Client.Common
{
    [TestClass]
    public abstract class TestControllerBase<Tarea> : TestIntegrationBase where Tarea : AreaRegistration, new()
    {
        public virtual void MyTestInitialize()
        {
            TestControllerBase<Tarea>.Application_InitEnterpriseLibrary();
            TestControllerBase<Tarea>.SetHttpContext();
        }

        private Tarea Area
        {
            get
            {
                return new Tarea();
            }
        }
    }
}
