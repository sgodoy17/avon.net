using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Transversal.Utilities
{
    public class DateUtils
    {
        public static DateTime? DateOrDefault(object val)
        {
            if (val == null)
                return null;

            string formattedValue =
                val.ToString()
                .Replace("a. ", "A").Replace("a.", "A")
                .Replace("p. ", "P").Replace("p.", "P")
                .Replace("a", "A").Replace("p", "P")
                .Replace("m.", "M").Replace("m", "M").Trim();

            DateTime? dateTimeValue = null;
            var converter = TypeDescriptor.GetConverter(typeof(DateTime?));

            try
            {
                DateTime posibleDate;
                if (DateTime.TryParse(val.ToString(), CultureInfo.CurrentCulture, DateTimeStyles.None, out posibleDate))
                    dateTimeValue = posibleDate;
            }
            catch
            {
                   dateTimeValue = (DateTime?)converter.ConvertFromInvariantString(formattedValue);
            }

            return dateTimeValue;
        }
    }
}
