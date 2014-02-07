using Microsoft.VisualStudio.TestTools.UnitTesting;
using $customNamespace$.Models.Globalization;
using $customNamespace$.Models.Unity;
using $customNamespace$.Models.UserRequestModel;
using $customNamespace$.UI.Web.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Hosting;

namespace $safeprojectname$
{
    public class TestCommon
    {
        //public static string CultureDefault = "en-US";
        //public static string CultureDefault = "es-ES";
        public static string CultureDefault = GlobalizationHelper.CultureInfoGetOrDefault(Thread.CurrentThread.CurrentCulture.Name).Name;
    }

    [TestClass]
    public abstract class TestBase
    {
        public static bool IsInitialized = false;
        public static string currentCultureName = TestCommon.CultureDefault;

        internal TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestInitialize()]
        public void TestBaseTestInitialize()
        {
            TestBase.Application_InitEnterpriseLibrary();
            TestBase.SetHttpContext();
        }

        [TestCleanup]
        public void TestBaseTestCleanUp()
        {

        }

        protected void CheckInheritance(Assembly assemblyToCheck, List<Type> excludedTypes, List<Type> baseClasses)
        {
            List<TypeInfo> assemblyTypes = assemblyToCheck.DefinedTypes.ToList();
            List<TypeInfo> assemblyTypesToCheck = assemblyTypes.Where(x => excludedTypes.All(p => p.FullName != x.FullName) && !x.IsEnum).ToList();
            List<TypeInfo> assemblyTypesDevelopment = assemblyTypesToCheck.Where(x =>
                                                                                !(x.IsDefined(typeof(CompilerGeneratedAttribute), false)) // filter out <>_DisplayClasses or any compiler generated class
                //&& !(x.GetCustomAttributes(typeof(CompilerGeneratedAttribute)).Count() == 0)
                                                                                ).ToList();

            //List<TypeInfo> assemblyTypesUnInherited = assemblyTypesDevelopment.Where(x => ((x.BaseType == null) || (!baseClasses.Select(p => p.FullName).Contains(x.BaseType.FullName)))).ToList();

            List<TypeInfo> assemblyTypesUnInherited = new List<TypeInfo>();

            foreach (var item in assemblyTypesDevelopment)
            {
                int inheritanceChildLevelCounter = 0;
                int inheritanceChildLevelMax = 10;
                bool inheritanceOK = false;
                Func<TypeInfo, bool> inheritanceValid = delegate(TypeInfo type)
                {
                    bool validBaseClass = baseClasses.Select(p => p.FullName).Contains(type.BaseType.FullName);
                    //return ((type.BaseType == null) || (!validBaseClass));
                    return ((type.BaseType != null) && (validBaseClass));
                };
                TypeInfo typeToCheck = item;
                while ((!inheritanceOK) && (typeToCheck != null) && (typeToCheck != typeof(object)) && (inheritanceChildLevelCounter < inheritanceChildLevelMax))
                {
                    inheritanceOK = inheritanceValid(typeToCheck);
                    if (!inheritanceOK)
                    {
                        typeToCheck = typeToCheck.BaseType.GetTypeInfo();
                    }

                    inheritanceChildLevelCounter++;
                }

                if (!inheritanceOK)
                {
                    assemblyTypesUnInherited.Add(item);
                }
            }


            Assert.AreEqual(0, assemblyTypesUnInherited.Count(),
                string.Format("\n \n {0} \n \n Contiene tipos que no heredan de ninguna de las clases base ({1}): \n \n {2}",
                                assemblyToCheck.ManifestModule.ToString(),
                                string.Join(", ", baseClasses.Select(x => x.Name)),
                                string.Join("\n,  ", assemblyTypesUnInherited.Select(x => x.FullName).ToList())));
        }

        public static void SetHttpContext()
        {
            SimpleWorkerRequest simpleWorkerRequest = new SimpleWorkerRequest(string.Empty, string.Empty, string.Empty, null, new StringWriter());
            HttpContext.Current = new HttpContext(simpleWorkerRequest);
            HttpContext.Current.Request.Cookies.Add(new HttpCookie(UserRequestModel_Keys.WcfClientCultureSelectedCookieName, currentCultureName));
            //HttpContext.Current.Request.Cookies.Add(new HttpCookie(UserRequestModel_Keys.WcfFormsAuthenticationCookieName, "sdsdsd"));
            Thread.CurrentThread.CurrentCulture = GlobalizationHelper.CultureInfoGetOrDefault(currentCultureName);
        }

        public static void Application_InitEnterpriseLibrary()
        {
            DependencyFactory.SetUnityContainerProviderFactory(UnityContainerProvider.GetContainer(DependencyFactory.UnityContainerDefault));
            //DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
            //LogWriterFactory logWriterFactory = new LogWriterFactory();
            //Logger.SetLogWriter(logWriterFactory.Create());
        }


    }
}
