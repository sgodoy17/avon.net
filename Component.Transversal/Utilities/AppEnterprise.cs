using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Transversal.Utilities
{
    public class AppEnterprise
    {
        public static AppEnterprise Current()
        {
            var enterprise = new AppEnterprise();

            var claims = from p in typeof(AppEnterprise).GetProperties()
                         select new
                         {
                             Name = p.Name,
                             Value = ClaimsUtils.GetClaimValue(string.Format(CustomClaimTypes.EnterpriseFieldFormat, p.Name))
                         };

            foreach (var c in claims)
            {
                if (!string.IsNullOrEmpty(c.Value))
                {
                    var propInfo = enterprise.GetType().GetProperty(c.Name);

                    try
                    {
                        if (propInfo.GetSetMethod() == null)
                            continue;

                        var tc = TypeDescriptor.GetConverter(propInfo.PropertyType);
                        object newValue = null;
                        if (tc.GetType() == typeof(DateTimeConverter) || (tc.GetType() == typeof(NullableConverter) && ((NullableConverter)tc).UnderlyingType == typeof(DateTime)))
                            newValue = DateUtils.DateOrDefault(c.Value);
                        else
                            newValue = tc.ConvertFrom(c.Value);

                        propInfo.SetValue(enterprise, newValue);
                    }
                    catch
                    {
                        try
                        {
                            object newValue = TypeDescriptor.GetConverter(propInfo.PropertyType).ConvertFromInvariantString(c.Value);
                            propInfo.SetValue(enterprise, newValue);
                        }
                        catch
                        {
                        }
                    }
                }
            }

            return enterprise;
        }

        public static bool IsValid()
        {
            var current = Current();
            int length = current.GetType().GetProperties().Length;

            var fields = from c in current.GetType().GetProperties()
                         select (c.GetMethod != null ? c.GetValue(current) : string.Empty);

            var fieldsWhite = from f in fields
                              where f == null || f.Equals(TypeUtils.GetDefault(f.GetType()))
                              select f;

            int count = fieldsWhite.Count();

            return length != count;
        }

        public int? Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
    }
}
