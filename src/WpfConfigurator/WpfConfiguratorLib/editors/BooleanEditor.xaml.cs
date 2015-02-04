using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WpfConfiguratorLib.editors
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
