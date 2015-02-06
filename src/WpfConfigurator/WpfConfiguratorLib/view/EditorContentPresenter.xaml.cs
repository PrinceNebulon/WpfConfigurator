using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace WpfConfiguratorLib.view
{
    /// <summary>
    /// Interaction logic for EditorContentPresenter.xaml
    /// </summary>
    public partial class EditorContentPresenter : UserControl
    {
        public static DependencyProperty LabelWidthPercentageProperty = DependencyProperty.Register(
            "LabelWidthPercentage", typeof(double), typeof(EditorContentPresenter), new PropertyMetadata((double)20));

        public double LabelWidthPercentage
        {
            get { return (double)GetValue(LabelWidthPercentageProperty); }
            set { SetValue(LabelWidthPercentageProperty, value); }
        }


        public EditorContentPresenter()
        {
            InitializeComponent();
        }
    }
}
