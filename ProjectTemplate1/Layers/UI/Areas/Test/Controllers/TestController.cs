using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using $customNamespace$.UI.Web.Areas.Test.Models;
using $customNamespace$.UI.Web.Controllers;

namespace $customNamespace$.UI.Web.Areas.Test.Controllers
{
    public class TestController : AsyncController, IControllerWithClientResources
    {
        public string[] GetControllerJavascriptResources
        {
            get
            {
                return new List<string>(){"~/Scripts/ui-widgetTreeList/ui-widgetTreeList.js",
                                        "~/Areas/Test/Content/Test_WidgetTree.js",

                                        "~/scripts/ui-widgetTreeListSort/ui-widgetTreeListSort.js",
                                        "~/Areas/Test/Content/Test_WidgetTreeSortable.js",

                                        "~/scripts/ui-widgetTreeListNest/ui-widgetTreeListNest.js",
                                        "~/Areas/Test/Content/Test_WidgetTreeNest.js"
                    
                }.ToArray();
            }
        }

        public string[] GetControllerStyleSheetResources
        {
            get
            {
                return new List<string>() { "~/Scripts/ui-widgetTreeList/ui-widgetTreeList.css" }.ToArray();
            }
        }

        public void IndexAsync(TestViewModel model)
        {
            AsyncManager.OutstandingOperations.Increment();

            Task.Factory.StartNew(() =>
            {
                

                if (this.Request.RequestType.ToLower() == HttpVerbs.Get.ToString().ToLower())
                {
                    model = new TestViewModel() { 
                        SomeBoolean = false,
                        SomeBooleanNullable = null,
                        SomeDouble = 67.89,
                        SomeDate = DateTime.Now
                    };
                }

                AsyncManager.Parameters["model"] = model;

                AsyncManager.OutstandingOperations.Decrement();
            });            
        }

        public ActionResult IndexCompleted(TestViewModel model)
        {
            model.BaseViewModelInfo.Title = "UI Controls";
            return View(model);
        }
    }
}
