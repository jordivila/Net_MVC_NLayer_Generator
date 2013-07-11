using System;
using System.ComponentModel.DataAnnotations;


namespace $safeprojectname$.DataAnnotationsAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DigitsAttribute : DataTypeAttribute
    {
        public DigitsAttribute()
            : base("digits")
        {
        }

        public override string FormatErrorMessage(string name)
        {
            //if (ErrorMessage == null && ErrorMessageResourceName == null)
            //{
            //    ErrorMessage = DataAnnotationsResources.DigitsAttribute_Invalid;
            //}
            return base.FormatErrorMessage(name);
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            long retNum;

            var parseSuccess = long.TryParse(Convert.ToString(value), out retNum);

            return parseSuccess && retNum >= 0;
        }
    }
}