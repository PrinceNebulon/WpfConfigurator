using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using WpfConfiguratorLib.Annotations;
using WpfConfiguratorLib.attributes;

namespace WpfConfiguratorLib.view
{
    /// <summary>
    /// Interaction logic for ConfigList.xaml
    /// </summary>
    public partial class ConfigList : INotifyPropertyChanged
    {
        public static DependencyProperty LabelWidthPercentageProperty = DependencyProperty.Register(
            "LabelWidthPercentage", typeof(double), typeof(ConfigList), new PropertyMetadata((double)20));

        public double LabelWidthPercentage
        {
            get { return (double)GetValue(LabelWidthPercentageProperty); }
            set { SetValue(LabelWidthPercentageProperty, value); }
        }

        private bool _isCollapsed = false;

        public bool IsCollapsed
        {
            get { return _isCollapsed; }
            set
            {
                _isCollapsed = value;
                OnPropertyChanged();
            }
        }


        public ConfigList()
        {
            InitializeComponent();
        }

        private void AddItem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var configListPropertyInfo = DataContext as ConfigListPropertyInfo;
                if (configListPropertyInfo != null)
                {
                    var list = (configListPropertyInfo.Value as IList);

                    // Add from default value
                    if (configListPropertyInfo.DefaultValue != null)
                        list.Add(configListPropertyInfo.DefaultValue);
                    // Check for primitive types
                    else if (configListPropertyInfo.DefaultListItemType == typeof (string))
                        list.Add(string.Empty);
                    else if (configListPropertyInfo.DefaultListItemType == typeof(bool))
                        list.Add(default(bool));
                    else if (configListPropertyInfo.DefaultListItemType == typeof(long))
                        list.Add(default(long));
                    else if (configListPropertyInfo.DefaultListItemType == typeof(float))
                        list.Add(default(float));
                    else if (configListPropertyInfo.DefaultListItemType == typeof(double))
                        list.Add(default(double));
                    else if (configListPropertyInfo.DefaultListItemType == typeof (int))
                        list.Add(default(int));
                    // Try to create dynamically
                    else
                    {
                        var item = Activator.CreateInstance(configListPropertyInfo.DefaultListItemType);
                        list.Add(item);
                    }

                    OnPropertyChanged("Value");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
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
