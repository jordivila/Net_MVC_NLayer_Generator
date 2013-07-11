using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace $safeprojectname$.Enumerations
{
    public static class EnumExtension
    {
        public static string EnumDescription(Enum enumerator)
        {
            string key = String.Format("{0}.{1}", enumerator.GetType(), enumerator).Replace(".", "_");
            string localizedDescription = $customNamespace$.Resources.Enumerations.EnumerationsTexts.ResourceManager.GetString(key);

            if (localizedDescription == null)
            {
                return enumerator.ToString();
            }
            else
            {
                return localizedDescription;
            }
        }
        public static IEnumerable<SelectListItem> ToSelectList(this Enum valueSelected, Type enumType)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            Array enumValues = Enum.GetValues(enumType);
            foreach (Enum item in enumValues)
            {
                list.Add(new SelectListItem()
                {
                    Selected = item.Equals(valueSelected),
                    Text = EnumExtension.EnumDescription(item),
                    Value = item.ToString()
                });
            }
            return list;
        }
        public static IEnumerable<SelectListItem> ToSelectList(this Enum valueSelected, Type enumType, Func<Enum, SelectListItem> forEachItem)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            Array enumValues = Enum.GetValues(enumType);
            foreach (Enum item in enumValues)
            {
                list.Add(forEachItem(item));
            }
            return list;
        }
        public static string ToUri(this ThemesAvailable themeSelected)
        {
            return string.Format("http://ajax.googleapis.com/ajax/libs/jqueryui/1/themes/{0}/jquery-ui.css", themeSelected.ToString().Replace("_", "-").ToLower());
        }
    }
}
