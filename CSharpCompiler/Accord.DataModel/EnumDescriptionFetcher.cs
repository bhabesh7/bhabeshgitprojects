using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accord.DataModel
{
    public static class EnumDescriptionFetcher
    {
        public static string GetDescription<T>(this T e) where T:IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);
                var enumNames = Enum.GetNames(type);
                foreach (var enumName in enumNames)
                {
                    if(enumName != e.ToString())
                    {
                        continue;
                    }
                    var memInfo = type.GetMember(enumName);
                    var descriptionAttribute = memInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() as DescriptionAttribute;

                    if (descriptionAttribute != null)
                    {
                        return descriptionAttribute.Description;
                    }
                }
            }

            //foreach (int val in values)
            //{
            //    if (val == e.ToInt32(CultureInfo.InvariantCulture))
            //    {
            //        var memInfo = type.GetMember(type.GetEnumName(val));
            //        var descriptionAttribute = memInfo[0]
            //            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            //            .FirstOrDefault() as DescriptionAttribute;

            //        if (descriptionAttribute != null)
            //        {
            //            return descriptionAttribute.Description;
            //        }
            //    }
            //}

            return string.Empty;
        }

    }
}

