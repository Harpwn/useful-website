using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UsefulCore.Enums
{
    public static class EnumExtensions
    {
        public static IEnumerable<TEnum> EnumsFromCSV<TEnum>(string csv)
            where TEnum : Enum
        {
            if (csv == null)
                return new List<TEnum>();

            var enumValues = ((TEnum[])Enum.GetValues(typeof(TEnum))).Select(e => Convert.ToInt32(e).ToString());
            var csvValues = csv.Split(',').Intersect(enumValues);

            return csvValues.Select(s => (TEnum)Enum.Parse(typeof(TEnum), s));
        }

        public static string CSVFromEnums<TEnum>(IEnumerable<TEnum> enumValues)
            where TEnum : Enum
        {
            return string.Join(',', enumValues.Select(e => Convert.ToInt32(e)));
        }

        public static string GetDisplayName(this Enum enu)
        {
            var attr = GetDisplayAttribute(enu);
            return attr != null ? attr.Name : enu.ToString();
        }

        public static string GetDescription(this Enum enu)
        {
            var attr = GetDisplayAttribute(enu);
            return attr != null ? attr.Description : enu.ToString();
        }

        private static DisplayAttribute GetDisplayAttribute(object value)
        {
            Type type = value.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException(string.Format("Type {0} is not an enum", type));
            }

            // Get the enum field.
            var field = type.GetField(value.ToString());
            return field == null ? null : field.GetCustomAttribute<DisplayAttribute>();
        }

    }
}
