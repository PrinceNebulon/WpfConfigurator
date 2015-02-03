using System;

namespace WpfConfiguratorLib.attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ConfigPropertyAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public object DefaultValue { get; set; }
        public bool IsPrivate { get; set; }


        public ConfigPropertyAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
