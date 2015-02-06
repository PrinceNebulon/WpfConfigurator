using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using Newtonsoft.Json;
using WpfConfiguratorLib.attributes;

namespace WpfConfiguratorLib.entities
{
    public abstract class ConfigGroup
    {
        #region Private Fields

        internal bool IsInitialized { get; set; }

        private SolidColorBrush _brush;

        #endregion



        #region Public Properties

        [JsonIgnore]
        public abstract string DisplayName { get; }

        [JsonIgnore]
        public string PropertyDisplayName { get; private set; }

        [JsonIgnore]
        public string MergedDisplayName
        {
            get { return string.IsNullOrEmpty(PropertyDisplayName) ? DisplayName : PropertyDisplayName; }
        }

        [JsonIgnore]
        public abstract string Description { get; }

        [JsonIgnore]
        public string PropertyDescription { get; private set; }

        [JsonIgnore]
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
                    var info = ConfigPropertyInfo.FromConfigPropertyAttribute(configPropertyAttribute,
                        propertyInfo.Name, GetValue(propertyInfo.Name), propertyInfo.PropertyType, OnValueChanged);

                    // Do list things
                    if (info is ConfigListPropertyInfo)
                    {
                        var listInfo = info as ConfigListPropertyInfo;

                        // Initialize empty list
                        if (listInfo.Value == null)
                        {
                            var list =
                                Activator.CreateInstance(
                                    (typeof (ObservableCollection<>).MakeGenericType(listInfo.DefaultListItemType)));
                            listInfo.Value = list;
                        }
                        else
                        {
                            foreach (var item in listInfo.Value as IEnumerable)
                            {
                                var configGroup = item as ConfigGroup;
                                if (configGroup != null)
                                    configGroup.IsInitialized = true;
                            }
                        }
                    }

                    // Add item to properties list
                    properties.Add(info);
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
                        // Create instance
                        propertyData = Activator.CreateInstance(propertyInfo.PropertyType) as ConfigGroup;

                        // Skip if we didn't create anything
                        if (propertyData == null) continue;

                        // Set the new value
                        OnValueChanged(propertyInfo.Name, propertyData);
                    }
                    else
                    {
                        // Mark as initialized since the property was retrieved
                        propertyData.IsInitialized = true;
                    }

                    // Override things
                    if (!string.IsNullOrEmpty(configPropertyAttribute.DisplayName))
                        propertyData.PropertyDisplayName = configPropertyAttribute.DisplayName;
                    if (!string.IsNullOrEmpty(configPropertyAttribute.Description))
                        propertyData.PropertyDescription = configPropertyAttribute.Description;
                    if (!string.IsNullOrEmpty(configPropertyAttribute.Color))
                    {
                        var color = ColorConverter.ConvertFromString(configPropertyAttribute.Color);
                        propertyData.Brush = color != null ? new SolidColorBrush((Color)color) : PickBrush();
                    }

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

            // Set default value since the object isn't initialized
            if (!IsInitialized)
            {
                // Get the ConfigPropertyAttribute attribute
                var configPropertyAttribute =
                    property.GetCustomAttributes().OfType<ConfigPropertyAttribute>().FirstOrDefault();

                var newValue = configPropertyAttribute != null
                    ? configPropertyAttribute.DefaultValue
                    : property.PropertyType.IsValueType
                        ? Activator.CreateInstance(property.PropertyType)
                        : null;
                if (newValue != null)
                {
                    OnValueChanged(propertyName, newValue);
                }
            }

            // Return set value
            var val = property.GetValue(this);
            return val;
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

        private void OnValueChanged(string propertyName, object value)
        {
            try
            {
                var property = GetType().GetProperty(propertyName);

                if (property.PropertyType == typeof(int))
                {
                    int intVal;
                    if (int.TryParse(value.ToString(), out intVal))
                    {
                        GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, this, new object[] { intVal });
                        return;
                    }
                }

                if (property.PropertyType == typeof(double))
                {
                    double doubleVal;
                    if (double.TryParse(value.ToString(), out doubleVal))
                    {
                        GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, this, new object[] { doubleVal });
                        return;
                    }
                }

                if (property.PropertyType == typeof(long))
                {
                    long longVal;
                    if (long.TryParse(value.ToString(), out longVal))
                    {
                        GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, this, new object[] { longVal });
                        return;
                    }
                }

                if (property.PropertyType == typeof(float))
                {
                    float floatVal;
                    if (float.TryParse(value.ToString(), out floatVal))
                    {
                        GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, this, new object[] { floatVal });
                        return;
                    }
                }

                // Default
                Console.WriteLine("Attempting to set {0} using the default invocation", propertyName);
                GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, this, new[] { value });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion



        #region Public Methods



        #endregion
    }
}
