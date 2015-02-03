using System;

namespace WpfConfiguratorLib.attributes
{
    public class ConfigPropertyInfo
    {
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

        private ValueChangedHandler _valueChangedDelegate;
        private object _value;

        private ConfigPropertyInfo()
        {
        }

        public static ConfigPropertyInfo FromConfigPropertyAttribute(ConfigPropertyAttribute attribute, string propertyName,
            object value, Type type, ValueChangedHandler valueChangedDelegate)
        {
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
    }
}