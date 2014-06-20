using System;
using System.Web.Mvc;
using $customNamespace$.UI.Web.Controllers;

namespace $customNamespace$.UI.Web.Common.Mvc.Attributes
{
    [AttributeUsage(
                                AttributeTargets.Class |
                                AttributeTargets.Enum |
                                AttributeTargets.Interface |
                                AttributeTargets.Parameter |
                                AttributeTargets.Struct |
                                AttributeTargets.Property,
                                AllowMultiple = false,
                                Inherited = false)]
    public class NonValidateModelOnHttpGetAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return new NonValidateModelOnHttpGetBinder();
        }
    }
    public class NonValidateModelOnHttpGetBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (controllerContext.Controller.RequestType() != HttpVerbs.Get)
            {
                return base.BindModel(controllerContext, bindingContext);
            }
            else
            {
                return base.CreateModel(controllerContext, bindingContext, bindingContext.ModelType);
            }
        }
    }
}