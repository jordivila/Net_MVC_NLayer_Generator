using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Runtime.Serialization;
using System.Linq;


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

        public static string ToEnumMemberString(this Enum valueSelected)
        {
            return EnumExtension.ToEnumMemberString<Enum>(valueSelected);
        }
        public static string ToEnumMemberString<T>(T type)
        {
            var enumType = type.GetType();
            var name = Enum.GetName(enumType, type);
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
            return enumMemberAttribute.Value;
        }
        
        public static Nullable<T> ToEnumMember<T>(string str) where T : struct
        {
            var enumType = typeof(T);
            foreach (var name in Enum.GetNames(enumType))
            {
                var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
                if (enumMemberAttribute.Value == str)
                {
                    return (T)Enum.Parse(enumType, name);
                }
            }

            return null;
        }
        public static Nullable<T> ToEnumMember<T>(string str, bool useDefaultValue) where T : struct
        {
            Nullable<T> result = EnumExtension.ToEnumMember<T>(str);
            
            if ((!result.HasValue) && useDefaultValue)
            {
                result = default(T);
            }

            return result;
        }


    }
}
