using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Component.Transversal.Utilities
{
    public static class ObjectHelper
    {

        /// <summary>
        /// Shallows the copy.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The source.</param>
        public static void ShallowCopy(object dest, object src)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            FieldInfo[] destFields = dest.GetType().GetFields(flags);
            FieldInfo[] srcFields = src.GetType().GetFields(flags);

            foreach (FieldInfo srcField in srcFields)
            {
                FieldInfo destField = destFields.FirstOrDefault(field => field.Name == srcField.Name);

                if (destField != null && !destField.IsLiteral)
                {
                    if (srcField.FieldType == destField.FieldType)
                        destField.SetValue(dest, srcField.GetValue(src));
                }
            }
        }

        //public void ShallowCopy(dynamic src, object fieldName)
        //{
        //    var field = fieldName.Split('.');
        //    foreach (PropertyDescriptor srcField in TypeDescriptor.GetProperties(src))
        //    {

        //        srcField.SetValue(srcField);
        //    }
        //}

        public static void SetProperty(object obj, string poperty, object objValue)
        {
            var property = obj.GetType().GetProperty(poperty);
            if (property != null)
                property.SetValue(obj, objValue, null);
        }

        public static List<string> GetFieldValueList(dynamic src)
        {
            List<string> fiedValue = new List<string>();
            foreach (PropertyDescriptor srcField in TypeDescriptor.GetProperties(src))
                fiedValue.Add(srcField.GetValue(src));
            return fiedValue;
        }

        public static List<string> GetFieldList(dynamic dest, Type ignore = null, Type validType = null)
        {
            var field = new List<string>();
            foreach (PropertyDescriptor srcField in TypeDescriptor.GetProperties(dest))
            {
                var type = srcField.PropertyType;

                if (type == ignore) continue;

                if (validType != null && validType != type) continue;

                var name = srcField.Name;

                if (type.IsPrimitive || type == typeof(decimal) || type == typeof(string))
                    field.Add(name);
                else
                    field.AddRange(from PropertyDescriptor srcFiledChild in TypeDescriptor.GetProperties(type) select string.Format("{0}.{1}", name, srcFiledChild.Name));
            }

            return field;
        }

        public static List<Tuple<string, string>> GetFieldDisplayList(dynamic dest, Type validType = null)
        {
            var field = new List<Tuple<string, string>>();
            foreach (PropertyDescriptor srcField in TypeDescriptor.GetProperties(dest))
            {
                var type = srcField.PropertyType;

                if (validType != null && validType != type) continue;

                var name = srcField.Name;
                var displayName = srcField.DisplayName;

                field.Add(new Tuple<string, string>(name, displayName));
            }

            return field;
        }

        public static List<Tuple<string, string>> GetFieldValueList(dynamic dest, Type validType = null)
        {
            var field = new List<Tuple<string, string>>();
            foreach (PropertyDescriptor srcField in TypeDescriptor.GetProperties(dest))
            {
                var type = srcField.PropertyType;

                if (validType != null && validType != type) continue;

                var name = srcField.Name;
                var value = srcField.GetValue(dest);

                field.Add(new Tuple<string, string>(name, value != null ? value.ToString() : string.Empty));
            }

            return field;
        }

        public static string GetFieldValue(dynamic src, string fieldName)
        {
            var field = fieldName.Split('.');
            foreach (var srcField in TypeDescriptor.GetProperties(src))
            {
                if (field.Length <= 1)
                {
                    if (srcField.Name != fieldName) continue;

                    var fieldValue = srcField.GetValue(src);

                    return fieldValue != null ? fieldValue.ToString() : null;
                }

                var type = srcField.PropertyType;

                if (type.Name != field[0]) continue;

                var srcChild = srcField.GetValue(src);

                if (srcChild == null) return null;

                foreach (var srcFieldChild in TypeDescriptor.GetProperties(srcChild))
                {
                    if (srcFieldChild.Name != field[1]) continue;

                    var fieldValue = srcFieldChild.GetValue(srcChild);

                    return fieldValue != null ? fieldValue.ToString() : null;
                }

                return string.Empty;
            }

            return string.Empty;
        }

        public static string GetFieldValue(dynamic src, string fieldName, Type typeField)
        {
            foreach (var srcField in TypeDescriptor.GetProperties(src))
            {
                var type = srcField.PropertyType;

                if (typeField != type)
                {

                    var srcChild = srcField.GetValue(src);

                    if (srcChild == null) return null;

                    foreach (var srcFieldChild in TypeDescriptor.GetProperties(srcChild))
                    {
                        if (srcFieldChild.Name != fieldName) continue;

                        var fieldValueChild = srcFieldChild.GetValue(srcChild);

                        return fieldValueChild != null ? fieldValueChild.ToString() : null;
                    }
                }

                if (srcField.Name != fieldName) continue;

                var fieldValue = srcField.GetValue(src);

                return fieldValue != null ? fieldValue.ToString() : null;
            }

            return string.Empty;
        }

        public static List<string> GetEnumValueField(Type type)
        {
            return type.GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
                .Select(p => p.GetValue(null)).Select(v => v.ToString()).ToList();
        }
    }
}
