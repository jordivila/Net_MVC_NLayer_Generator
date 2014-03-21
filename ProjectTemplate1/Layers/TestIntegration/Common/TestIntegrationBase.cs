using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using $customNamespace$.Models.Globalization;
using $customNamespace$.Models.Unity;
using $customNamespace$.Models.UserRequestModel;
using $customNamespace$.Tests.Common;
using $customNamespace$.UI.Web.Unity;

namespace $customNamespace$.Tests.Integration.Common
{
    public class TestIntegrationBase : TestBase
    {
        [TestInitialize()]
        public void TestBaseTestInitialize()
        {
            TestIntegrationBase.TestBaseHostWCFInit(this.TestContext);
            TestIntegrationBase.Application_InitEnterpriseLibrary();
            TestIntegrationBase.SetHttpContext();
        }

        [TestCleanup]
        public void TestBaseTestCleanUp()
        {
            TestIntegrationBase.TestBaseHostWCFEnd();
        }

        public static void SetHttpContext()
        {
            SimpleWorkerRequest simpleWorkerRequest = new SimpleWorkerRequest(string.Empty, string.Empty, string.Empty, null, new StringWriter());
            HttpContext.Current = new HttpContext(simpleWorkerRequest);
            HttpContext.Current.Request.Cookies.Add(new HttpCookie(UserRequestModel_Keys.WcfClientCultureSelectedCookieName, currentCultureName));
            Thread.CurrentThread.CurrentCulture = GlobalizationHelper.CultureInfoGetOrDefault(currentCultureName);
        }

        public static void Application_InitEnterpriseLibrary()
        {
            DependencyFactory.SetUnityContainerProviderFactory(UnityContainerProvider.GetContainer(FrontEndUnityContainerAvailable.ProxiesToCustomHost));
            //DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
            //LogWriterFactory logWriterFactory = new LogWriterFactory();
            //Logger.SetLogWriter(logWriterFactory.Create());
        }

        #region Self Hosted WCF
        private static object lockObject = new object();
        private static Process TestBaseHostWCFHostProcess = null;
        private static void TestBaseHostWCFInit(TestContext testContext)
        {
            lock (lockObject)
            {
                if (TestBaseHostWCFHostProcess == null)
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.CreateNoWindow = false;
                    startInfo.UseShellExecute = false;
                    startInfo.FileName = Path.Combine(testContext.DeploymentDirectory, "$customNamespace$.WCF.ServicesHost.exe");
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    //startInfo.Arguments = "-f j -o \"" + ex1 + "\" -z 1.0 -s y " + ex2;

                    try
                    {
                        TestBaseHostWCFHostProcess = Process.Start(startInfo);

                        // Start the process with the info we specified.
                        // Call WaitForExit and then the using statement will close.
                        //using (Process exeProcess = Process.Start(startInfo))
                        //{
                        //exeProcess.WaitForExit();
                        //}
                    }
                    catch (Exception ex)
                    {
                        Assert.Inconclusive(ex.Message);
                    }
                }
            }
        }
        private static void TestBaseHostWCFEnd()
        {
            lock (lockObject)
            {
                if (TestBaseHostWCFHostProcess != null)
                {
                    TestBaseHostWCFHostProcess.Close();
                    TestBaseHostWCFHostProcess.Dispose();
                }
            }
        }
        #endregion
    }
}
