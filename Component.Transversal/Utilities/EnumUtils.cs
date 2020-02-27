using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Component.Transversal.Utilities
{
    public static class EnumUtils
    {

        public static string DisplayName(Enum value)
        {
            Type enumType = value.GetType();
            var enumValue = Enum.GetName(enumType, value);
            MemberInfo member = enumType.GetMember(enumValue)[0];

            var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            var outString = ((DisplayAttribute)attrs[0]).Name;

            if (((DisplayAttribute)attrs[0]).ResourceType != null)
            {
                outString = ((DisplayAttribute)attrs[0]).GetName();
            }

            return outString;
        }
        public static SelectList GetSelectListEnum<T>(List<T> enumList, T selected)
        {
            return new SelectList(enumList.Select(
                e => new SelectListItem() { Text = DisplayName((dynamic)e), Value = ((int)(dynamic)e).ToString() }), "Value", "Text", selected);
        }
    }
}
