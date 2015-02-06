using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using WpfConfiguratorLib.Annotations;

namespace WpfConfiguratorLib.view
{
    /// <summary>
    /// Interaction logic for ConfigurationGroup.xaml
    /// </summary>
    public partial class ConfigurationGroup : INotifyPropertyChanged
    {
        public static DependencyProperty LabelWidthPercentageProperty = DependencyProperty.Register(
            "LabelWidthPercentage", typeof(double), typeof(ConfigurationGroup), new PropertyMetadata((double)20));

        private bool _isCollapsed = false;

        public double LabelWidthPercentage
        {
            get { return (double)GetValue(LabelWidthPercentageProperty); }
            set { SetValue(LabelWidthPercentageProperty, value); }
        }

        public bool IsCollapsed
        {
            get { return _isCollapsed; }
            set
            {
                _isCollapsed = value;
                OnPropertyChanged();
            }
        }

        public ConfigurationGroup()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ExpandCollapse_OnClick(object sender, RoutedEventArgs e)
        {
            IsCollapsed = !IsCollapsed;
        }
    }
}
