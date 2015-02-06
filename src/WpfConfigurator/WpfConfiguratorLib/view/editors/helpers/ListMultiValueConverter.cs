using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfConfiguratorLib.view.editors.helpers
{
    public class ListMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType,
               object parameter, System.Globalization.CultureInfo culture)
        {
            var valuesList = new ArrayList();
            
            foreach (var value in values)
            {
                if (IsSubclassOfRawGeneric(typeof(ICollection), value.GetType()))
                {
                    var iList = value as ICollection;
                    if (iList != null)
                        valuesList.AddRange(iList);
                }
                else
                {
                    valuesList.Add(value);
                }
            }

            return valuesList;
        }
        public object[] ConvertBack(object value, Type[] targetTypes,
               object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }

        private bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;

                // Match on type def?
                if (generic == cur)
                {
                    return true;
                }

                // Match on interface implementation?
                var interfaces = cur.GetInterfaces();
                if (interfaces.Any(i => i.GUID == generic.GUID))
                    return true;

                // Check against base type
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}
