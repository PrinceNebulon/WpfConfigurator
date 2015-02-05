using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using WpfConfiguratorLib.attributes;

namespace WpfConfiguratorLib.view.editors
{
    /// <summary>
    /// Interaction logic for ComboboxEditor.xaml
    /// </summary>
    public partial class ComboboxEditor : UserControl
    {
        public static DependencyProperty LabelWidthPercentageProperty = DependencyProperty.Register(
            "LabelWidthPercentage", typeof(double), typeof(ComboboxEditor), new PropertyMetadata((double)20));

        public double LabelWidthPercentage
        {
            get { return (double)GetValue(LabelWidthPercentageProperty); }
            set { SetValue(LabelWidthPercentageProperty, value); }
        }


        public ComboboxEditor()
        {
            DataContextChanged += OnDataContextChanged;
            InitializeComponent();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            try
            {
                var data = DataContext as ConfigPropertyInfo;
                if (data == null ||
                    !data.Type.IsEnum) return;

                foreach (var value in Enum.GetValues(data.Type))
                {
                    //var description = Enum.GetName(data.Type, value);
                    var description = GetEnumDescription(data.Type, value);
                    Box.Items.Add(new Tuple<string, object>(description, value));
                }

                Box.SelectedValue = data.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private string GetEnumDescription(Type type, object value)
        {
            var fi = type.GetField(value.ToString());

            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
