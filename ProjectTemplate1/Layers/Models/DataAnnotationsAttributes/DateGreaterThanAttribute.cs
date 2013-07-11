using System;
using System.ComponentModel.DataAnnotations;
/*using System.Web.Mvc;*/
using System.Globalization;
using System.Reflection;


namespace $safeprojectname$.DataAnnotationsAttributes
{

    [AttributeUsage(AttributeTargets.Property)]
    public class DateGreaterThanAttribute : ValidationAttribute/*, IClientValidatable*/
    {
        public DateGreaterThanAttribute(string otherProperty)
        {
            if (otherProperty == null)
            {
                throw new ArgumentNullException("otherProperty");
            }
            OtherProperty = otherProperty;
        }

        public string OtherProperty { get; private set; }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, OtherProperty);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
            if (otherPropertyInfo == null)
            {
                throw new ArgumentException();
            }

            object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

            if ((value != null) && (otherPropertyValue != null))
            {
                if ((DateTime)otherPropertyValue > ((DateTime)value))
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }

            return null;
        }

        public static string FormatPropertyForClientValidation(string property)
        {
            if (property == null)
            {
                throw new ArgumentException();
            }
            return "*." + property;
        }

        //public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        //{
        //    yield return new ModelClientValidationEqualToRule(FormatErrorMessage(metadata.GetDisplayName()), FormatPropertyForClientValidation(OtherProperty));
        //}

    }

}