using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Transversal.Utilities
{
    public class TypeUtils
    {
        public static object GetDefault(Type t)
        {
            if (t.IsValueType)
                return Activator.CreateInstance(t);
            return null;
        }

        public static T GetDefaultGeneric<T>()
        {
            return default(T);
        }
    }
}
