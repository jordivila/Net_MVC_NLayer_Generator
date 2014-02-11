using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace $customNamespace$.Tests.Integration.Common.Actions
{
    public static class Helper<TModel, TProperty>
    {

        public static bool PropertyHasAttribute(TModel model, Expression<Func<TModel, TProperty>> expression, Type attributeType)
        {
            return model.GetType().GetProperty(ExpressionHelper.GetExpressionText(expression)).GetCustomAttributes(attributeType, false).Count() > 0;
        }

        public static bool PropertyHasDataType(TModel model, Expression<Func<TModel, TProperty>> expression, DataType dataType)
        {
            return ((DataTypeAttribute)model.GetType().GetProperty(ExpressionHelper.GetExpressionText(expression)).GetCustomAttributes(typeof(DataTypeAttribute), false).First()).DataType == dataType;
        }

    }
}
