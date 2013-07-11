using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace $safeprojectname$.TestCases.Configuration
{
    [TestClass]
    public class ConfigurationTests
    {
        public ConfigurationTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

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

        [TestMethod]
        public void UnityTest_InterfaceConfiguration()
        {
            UnityConfigurationSection unitySection = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            IEnumerable<ContainerElement> ces = unitySection.Containers;

            List<string> allInterfaces = (from t in unitySection.TypeAliases
                                          where Type.GetType(t.TypeName).IsInterface
                                          select t.Alias).ToList();

            foreach (var ce in ces)
            {
                List<string> containerInterfaces = (from r in ce.Registrations select r.TypeName).ToList();

                Assert.AreEqual(true,
                                            allInterfaces.Except(containerInterfaces).ToList().Count == 0,
                                            string.Format("Dentro del container '{0}' existe algun alias sin configurar: {1}",
                                                            ce.Name,
                                                            string.Join(",", allInterfaces.Except(containerInterfaces).ToList()))
                                                            );

                using (IUnityContainer dependencyFactory = new UnityContainer())
                {
                    if (string.IsNullOrEmpty(ce.Name))
                    {
                        dependencyFactory.LoadConfiguration();
                    }
                    else
                    {
                        dependencyFactory.LoadConfiguration(ce.Name);
                    }

                    //Comprueba que el mapping esta bien hecho para todos los elementos del container
                    foreach (var registration in ce.Registrations)
                    {
                        Type interfaceType = Type.GetType(unitySection.TypeAliases[registration.TypeName]);
                        object injectedResult = dependencyFactory.Resolve(interfaceType, null);

                        Assert.AreEqual(true,
                                                    Type.GetType(unitySection.TypeAliases[registration.MapToName]) == injectedResult.GetType(),
                                                    string.Format("Interface {0} dentro del contenedor {1} no concuerda con el resultado obtenido {2}", registration.TypeName, ce.Name, injectedResult));
                    }
                }
            }
        }


        [TestMethod]
        public void ClientConfigTest()
        {
            Assert.IsTrue($customNamespace$.Models.Configuration.ApplicationConfiguration.ClientResourcesSettingsSection.WebSiteCommonScripts.Count > 0);
            Assert.IsTrue($customNamespace$.Models.Configuration.ApplicationConfiguration.ClientResourcesSettingsSection.JQueryLibScripts.Count > 0);
            Assert.IsTrue($customNamespace$.Models.Configuration.ApplicationConfiguration.ClientResourcesSettingsSection.WebSitePageInitScripts.Count > 0);
            
            //Assert.IsTrue($customNamespace$.Models.Configuration.ApplicationConfiguration.ClientResourcesSettingsSection.JQueryLibGlobalizeLocalization.IndexOf("{0}")> -1);
            //Assert.IsTrue($customNamespace$.Models.Configuration.ApplicationConfiguration.ClientResourcesSettingsSection.JQueryLibValidateLocalization.IndexOf("{0}") > -1);
            //Assert.IsTrue($customNamespace$.Models.Configuration.ApplicationConfiguration.ClientResourcesSettingsSection.JQueryLibUILocalization.IndexOf("{0}") > -1);
        }
    }
}
