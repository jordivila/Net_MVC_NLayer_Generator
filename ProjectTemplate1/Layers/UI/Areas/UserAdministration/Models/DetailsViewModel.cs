using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using $customNamespace$.Models;
using $customNamespace$.Models.Common;
using $customNamespace$.Models.Membership;
using $customNamespace$.UI.Web.Controllers;
using $customNamespace$.UI.Web.Models;

namespace $customNamespace$.UI.Web.Areas.UserAdministration.Models
{
	public class DetailsViewModel: baseViewModel
	{
        public DetailsViewModel()
        {

        }

        public MembershipUserWrapper UserOriginal { get; set; }
        public MembershipUserWrapper UserUpdated { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public DataResultBoolean ResultLastAction { get; set; }
	}

    public class DetailsViewModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            
            DetailsViewModel o = (DetailsViewModel)base.BindModel(controllerContext, bindingContext);
            if (ControllerHelper.RequestType(controllerContext) == HttpVerbs.Post)
            {
                Expression<Func<DetailsViewModel, MembershipUserWrapper>> expression = m => ((DetailsViewModel)bindingContext.Model).UserOriginal;
                string expressionText = ExpressionHelper.GetExpressionText(expression);
                string jsonUserDetails = bindingContext.ValueProvider.GetValue(expressionText).AttemptedValue;
                o.UserOriginal = baseModel.DeserializeFromJson<MembershipUserWrapper>(jsonUserDetails);
            }
            return o;
        }
    }

}