using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
using WpfConfiguratorLib.attributes;

namespace WpfConfiguratorLib.editors
{
    /// <summary>
    /// Interaction logic for ComboboxEditor.xaml
    /// </summary>
    public partial class ComboboxEditor : UserControl
    {
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
