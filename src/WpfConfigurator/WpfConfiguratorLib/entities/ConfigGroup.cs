using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using Newtonsoft.Json;
using WpfConfiguratorLib.attributes;

namespace WpfConfiguratorLib.entities
{
    public abstract class ConfigGroup
    {
        #region Private Fields

        private void ValueChangedDelegate(string propertyName, object value)
        {
            try
            {
                GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, this, new[] { value });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private SolidColorBrush _brush;

        #endregion



        #region Public Properties

        public abstract string DisplayName { get; }

        public string PropertyDisplayName { get; set; }

        public string MergedDisplayName
        {
            get { return string.IsNullOrEmpty(PropertyDisplayName) ? DisplayName : PropertyDisplayName; }
        }

        public abstract string Description { get; }

        public string PropertyDescription { get; set; }

        public string MergedDescription
        {
            get { return string.IsNullOrEmpty(PropertyDescription) ? Description : PropertyDescription; }
        }

        public virtual SolidColorBrush Brush
        {
            get { return _brush; }
            set { _brush = value; }
        }

        [JsonIgnore]
        public ReadOnlyCollection<ConfigPropertyInfo> Properties
        {
            get
            {
                var properties = new List<ConfigPropertyInfo>();

                // Iterate through properties
                foreach (var propertyInfo in GetType().GetProperties())
                {
                    // Get ConfigPropertyAttribute attribute
                    var configPropertyAttribute = propertyInfo.GetCustomAttributes(true)
                        .OfType<ConfigPropertyAttribute>()
                        .FirstOrDefault();

                    // Validation conditions
                    if (propertyInfo.PropertyType.IsSubclassOf(typeof(ConfigGroup))) continue;
                    if (configPropertyAttribute == null) continue;
                    if (configPropertyAttribute is ConfigGroupPropertyAttribute) continue;
                    if (configPropertyAttribute.IsPrivate) continue;

                    // Add to list
                    properties.Add(ConfigPropertyInfo.FromConfigPropertyAttribute(configPropertyAttribute,
                        propertyInfo.Name, GetValue(propertyInfo.Name), propertyInfo.GetType(), ValueChangedDelegate));
                }

                // Return list of properties
                return new ReadOnlyCollection<ConfigPropertyInfo>(properties);
            }
        }

        [JsonIgnore]
        public ReadOnlyCollection<ConfigGroup> Children
        {
            get
            {
                var configGroups = new List<ConfigGroup>();

                // Iterate through properties
                foreach (var propertyInfo in GetType().GetProperties())
                {
                    // Get ConfigPropertyAttribute attribute
                    var configPropertyAttribute = propertyInfo.GetCustomAttributes(true)
                        .OfType<ConfigGroupPropertyAttribute>()
                        .FirstOrDefault();

                    // Validation conditions
                    if (!propertyInfo.PropertyType.IsSubclassOf(typeof(ConfigGroup))) continue;
                    if (configPropertyAttribute == null) continue;
                    if (configPropertyAttribute.IsPrivate) continue;

                    // Get value
                    var propertyData = GetValue(propertyInfo.Name) as ConfigGroup;

                    // Create new instance if we failed to get a value (expected: unset object)
                    if (propertyData == null)
                    {
                        propertyData = Activator.CreateInstance(propertyInfo.PropertyType) as ConfigGroup;
                        ValueChangedDelegate(propertyInfo.Name, propertyData);
                    }

                    // Override things
                    if (!string.IsNullOrEmpty(configPropertyAttribute.DisplayName))
                        propertyData.PropertyDisplayName = configPropertyAttribute.DisplayName;
                    if (!string.IsNullOrEmpty(configPropertyAttribute.Description))
                        propertyData.PropertyDescription = configPropertyAttribute.Description;
                    if (!string.IsNullOrEmpty(configPropertyAttribute.Color))
                        propertyData.Brush =
                            new SolidColorBrush((Color) ColorConverter.ConvertFromString(configPropertyAttribute.Color));

                    // Add the item to the collection
                    configGroups.Add(propertyData);
                }

                // Return list of config groups
                return new ReadOnlyCollection<ConfigGroup>(configGroups);
            }
        }

        #endregion



        public ConfigGroup()
        {
            _brush = PickBrush();
        }



        #region Private Methods

        private object GetValue(string propertyName)
        {
            // Get property
            var property = GetType().GetProperty(propertyName);

            // Try to return from set values
            var value  = property.GetValue(this);
            if (value != null) return value;

            // Get the ConfigPropertyAttribute attribute
            var configPropertyAttribute =
                property.GetCustomAttributes().OfType<ConfigPropertyAttribute>().FirstOrDefault();

            // Return the attribute's specified default value or default value of the property's type
            return configPropertyAttribute != null
                ? configPropertyAttribute.DefaultValue
                : property.PropertyType.IsValueType
                    ? Activator.CreateInstance(property.PropertyType)
                    : null;
        }

        private SolidColorBrush PickBrush()
        {
            var rnd = new Random();
            return
                new SolidColorBrush(Color.FromRgb(
                    (byte) rnd.Next(0, 255),
                    (byte) rnd.Next(0, 255),
                    (byte) rnd.Next(0, 255)));
        }

        #endregion



        #region Public Methods



        #endregion
    }
}
