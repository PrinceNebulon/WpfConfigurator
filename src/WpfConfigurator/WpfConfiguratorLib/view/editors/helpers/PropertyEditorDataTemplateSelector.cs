using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfConfiguratorLib.attributes;
using WpfConfiguratorLib.entities;

namespace WpfConfiguratorLib.view.editors.helpers
{
    public class PropertyEditorDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            try
            {
                // Get objects
                var elem = container as FrameworkElement;
                if (elem == null) return null;

                // Determine template by item type
                if (item.GetType().IsSubclassOf(typeof(ConfigGroup)))
                    return elem.FindResource("ConfigurationGroup") as DataTemplate;
                if (item is ConfigListPropertyInfo)
                    return elem.FindResource("ConfigList") as DataTemplate;
                if (IsSubclassOfRawGeneric(typeof(Observable<>), item.GetType()))
                    return elem.FindResource("BasicStringEditor") as DataTemplate;
                

                // Determine template by config property type
                var data = item as ConfigPropertyInfo;
                if (data == null) return null;

                if (data.Type == typeof(string))
                    return elem.FindResource("StringEditor") as DataTemplate;
                if (data.Type == typeof(bool))
                    return elem.FindResource("BooleanEditor") as DataTemplate;
                if (data.Type.IsEnum)
                    return elem.FindResource("ComboboxEditor") as DataTemplate;
                if (IsNumericType(data.Type))
                    return elem.FindResource("NumericEditor") as DataTemplate;
                if (IsSubclassOfRawGeneric(typeof(ICollection), data.Type))
                    return elem.FindResource("ConfigList") as DataTemplate;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }

        private bool IsNumericType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
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
