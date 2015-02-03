namespace WpfConfiguratorLib.attributes
{
    public class ConfigGroupPropertyAttribute : ConfigPropertyAttribute
    {
        public string Color { get; set; }

        public ConfigGroupPropertyAttribute(string displayName) : base(displayName)
        {
            DisplayName = displayName;
        }
    }
}