using System;
using System.Windows.Media;

namespace WpfConfiguratorLib.attributes
{
    public class ConfigListPropertyInfo : ConfigPropertyInfo
    {
        public Type DefaultListItemType { get; set; }
        public SolidColorBrush Brush { get; set; }
    }
}