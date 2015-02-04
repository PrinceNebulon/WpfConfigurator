using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfConfiguratorLib.Annotations;

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
