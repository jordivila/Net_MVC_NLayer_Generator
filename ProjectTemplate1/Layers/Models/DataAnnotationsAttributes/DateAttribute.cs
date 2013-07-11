using System;
using System.ComponentModel.DataAnnotations;


namespace $safeprojectname$.DataAnnotationsAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateAttribute : DataTypeAttribute
    {
        public DateAttribute(): base(DataType.Date)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            DateTime retDate;

            return DateTime.TryParse(Convert.ToString(value), out retDate);
        }
    }
}