using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using $customNamespace$.UI.Web;

namespace $safeprojectname$.TestCases.GenericTests
{
    public static class StringArrayHelper
    {
        public static string CsvSplit(this String[] source)
        {
            return String.Join("\n", source);
        }
    }


    [TestClass]
    public class Ensure_AuthorizeAttribute
    {

        [TestMethod]
        public void Ensure_AuthorizeAttribute_IsInUse()
        {
            //Check all classes uses our custom Authorize Attribute instead System.Web.Mvc.AuthorizeAttribute
            Assembly assembly = Assembly.GetAssembly(typeof(MvcApplication));
            var classesWithAutohorize = (from t in assembly.GetTypes()
                                                where (t.GetCustomAttributes(typeof(System.Web.Mvc.AuthorizeAttribute), true).Count() > 0)
                                                select t).ToList();
            Assert.AreEqual(0, classesWithAutohorize.Count, string.Format("Existing types using the wrong Authorize attribute: \n {0}", classesWithAutohorize.Select(t => string.Format("{0},", t.Name)).ToArray().CsvSplit()));

            var methodsWithAutorize = (from t in assembly.GetTypes()
                                       from m in t.GetMethods()
                                       where m.GetCustomAttributes(typeof(System.Web.Mvc.AuthorizeAttribute), true).Count() > 0
                                       select new
                                       {
                                           t,
                                           m
                                       }).ToList();
            Assert.AreEqual(0, methodsWithAutorize.Count, string.Format("Existing types using the wrong Authorize attribute: {0} \n in Methods: {1}", 
                                                                                                                        methodsWithAutorize.Select(t => string.Format("{0},", t.t.FullName)).ToArray().CsvSplit(),
                                                                                                                        methodsWithAutorize.Select(t => string.Format("{0},", t.m.Name)).ToArray().CsvSplit()
                                                                                                                        ));
        }
    }
}
