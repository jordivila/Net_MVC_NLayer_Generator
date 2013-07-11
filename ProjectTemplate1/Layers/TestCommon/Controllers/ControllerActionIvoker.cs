using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace $safeprojectname$.Controllers
{
    public class ControllerActionInvokerForTesting : ControllerActionInvoker 
    {
        public ActionResult ActionResultExpected { get; set; }
        public Func<ActionResult, ActionResult, bool> ActionResultComparer { get; set; }

        protected override void InvokeActionResult(ControllerContext controllerContext, ActionResult actionResult)
        {
            if (this.ActionResultComparer == null)
            {
                Assert.IsTrue(actionResult == this.ActionResultExpected);
            }
            else
            {
                Assert.IsTrue(ActionResultComparer(actionResult, this.ActionResultExpected));
            }
        }
    }
}
