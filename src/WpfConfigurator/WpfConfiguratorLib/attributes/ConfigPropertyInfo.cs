using System;
using System.Windows.Media;

namespace WpfConfiguratorLib.attributes
{
    public class ConfigPropertyInfo
    {
        private ValueChangedHandler _valueChangedDelegate;
        private object _value;
        private static readonly object SyncObj = new object();
        private static Random _random = new Random();

        public delegate void ValueChangedHandler(string propertyName, object value);

        public string PropertyName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public object DefaultValue { get; set; }


        public object Value
        {
            get { return _value; }
            set
            {
                // Suppress unnecessary sets
                if (_value == value) return;

                // Set value
                _value = value;

                // Invoke the delegate
                if (_valueChangedDelegate != null) _valueChangedDelegate.Invoke(PropertyName, value);
            }
        }

        public Type Type { get; set; }

        protected ConfigPropertyInfo()
        {
        }

        public static ConfigPropertyInfo FromConfigPropertyAttribute(ConfigPropertyAttribute attribute, string propertyName,
            object value, Type type, ValueChangedHandler valueChangedDelegate)
        {

            // Process list types
            var attributeList = attribute as ConfigListPropertyAttribute;
            if (attributeList != null)
            {
                var info = new ConfigListPropertyInfo
                {
                    DisplayName = attribute.DisplayName,
                    PropertyName = propertyName,
                    Description = attribute.Description,
                    DefaultValue = attribute.DefaultValue,
                    Value = value,
                    Type = type,
                    _valueChangedDelegate = valueChangedDelegate
                };

                // Set brush color
                var color = ColorConverter.ConvertFromString(attributeList.Color);
                info.Brush = color != null ? new SolidColorBrush((Color) color) : PickBrush();

                // Determine default item type
                if (info.Type.GenericTypeArguments.Length > 0)
                {
                    info.DefaultListItemType = info.Type.GenericTypeArguments[0];
                }

                return info;
            }

            // Return default object
            return new ConfigPropertyInfo
            {
                DisplayName = attribute.DisplayName,
                PropertyName = propertyName,
                Description = attribute.Description,
                DefaultValue = attribute.DefaultValue,
                Value = value,
                Type = type,
                _valueChangedDelegate = valueChangedDelegate
            };
        }

        private static SolidColorBrush PickBrush()
        {
            lock (SyncObj)
            {
                var c1 = _random.Next(0, 255);
                var c2 = _random.Next(0, 255);
                var c3 = _random.Next(0, 255);
                Console.WriteLine("Color={0},{1},{2}", c1, c2, c3);
                return
                    new SolidColorBrush(Color.FromRgb((byte) c1, (byte) c2, (byte) c3));
            }
        }
    }
}