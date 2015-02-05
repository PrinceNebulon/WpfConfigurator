using System;
using System.Windows;
using System.Windows.Controls;
using WpfConfiguratorLib.attributes;

namespace WpfConfiguratorLib.editors.helpers
{
    public class PropertyEditorDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            try
            {
                var elem = container as FrameworkElement;
                var data = item as ConfigPropertyInfo;

                if (elem == null || data == null) return null;

                if (data.Type == typeof(string))
                    return elem.FindResource("StringEditor") as DataTemplate;
                if (data.Type == typeof(bool))
                    return elem.FindResource("BooleanEditor") as DataTemplate;
                if (data.Type.IsEnum)
                    return elem.FindResource("ComboboxEditor") as DataTemplate;
                if (IsNumericType(data.Type))
                    return elem.FindResource("NumericEditor") as DataTemplate;
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
    }
}
