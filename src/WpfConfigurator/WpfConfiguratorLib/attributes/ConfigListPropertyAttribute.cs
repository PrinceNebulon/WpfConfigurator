using System;

namespace WpfConfiguratorLib.attributes
{
    public class ConfigListPropertyAttribute : ConfigPropertyAttribute
    {
        public string Color { get; set; }
        //public Type DefaultListItemType { get; set; }

        public ConfigListPropertyAttribute(string displayName)
            : base(displayName)
        {
            DisplayName = displayName;
        }
    }
}