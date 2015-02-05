using System.Windows;
using System.Windows.Controls;

namespace WpfConfiguratorLib.view.editors
{
    /// <summary>
    /// Interaction logic for BooleanEditor.xaml
    /// </summary>
    public partial class BooleanEditor : UserControl
    {
        public static DependencyProperty LabelWidthPercentageProperty = DependencyProperty.Register(
            "LabelWidthPercentage", typeof(double), typeof(BooleanEditor), new PropertyMetadata((double)20));

        public double LabelWidthPercentage
        {
            get { return (double)GetValue(LabelWidthPercentageProperty); }
            set { SetValue(LabelWidthPercentageProperty, value); }
        }


        public BooleanEditor()
        {
            InitializeComponent();
        }
    }
}
