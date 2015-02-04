using System.Windows;
using System.Windows.Controls;

namespace WpfConfiguratorLib
{
    /// <summary>
    /// Interaction logic for ConfigurationGroup.xaml
    /// </summary>
    public partial class ConfigurationGroup : UserControl
    {
        public static DependencyProperty LabelWidthPercentageProperty = DependencyProperty.Register(
            "LabelWidthPercentage", typeof(double), typeof(ConfigurationGroup), new PropertyMetadata((double)20));

        public double LabelWidthPercentage
        {
            get { return (double)GetValue(LabelWidthPercentageProperty); }
            set { SetValue(LabelWidthPercentageProperty, value); }
        }

        public ConfigurationGroup()
        {
            InitializeComponent();
        }
    }
}
