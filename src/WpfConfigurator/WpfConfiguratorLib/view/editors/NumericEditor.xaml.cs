using System.Windows;
using System.Windows.Controls;

namespace WpfConfiguratorLib.view.editors
{
    /// <summary>
    /// Interaction logic for NumericEditor.xaml
    /// </summary>
    public partial class NumericEditor : UserControl
    {
        public static DependencyProperty LabelWidthPercentageProperty = DependencyProperty.Register(
            "LabelWidthPercentage", typeof(double), typeof(NumericEditor), new PropertyMetadata((double)20));

        public double LabelWidthPercentage
        {
            get { return (double)GetValue(LabelWidthPercentageProperty); }
            set { SetValue(LabelWidthPercentageProperty, value); }
        }


        public NumericEditor()
        {
            InitializeComponent();
        }
    }
}
