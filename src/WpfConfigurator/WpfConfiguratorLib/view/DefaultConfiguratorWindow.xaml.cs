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
using System.Windows.Shapes;
using WpfConfiguratorLib.entities;

namespace WpfConfiguratorLib.view
{
    /// <summary>
    /// Interaction logic for DefaultConfiguratorWindow.xaml
    /// </summary>
    public partial class DefaultConfiguratorWindow : Window
    {
        public DefaultConfiguratorWindow(string displayName, Type targetType)
        {
            InitializeComponent();

            try
            {
                // Load config from disk or create new
                WpfConfigurator.DataContext = ConfigManager.Load(displayName, targetType) ??
                                              Activator.CreateInstance(targetType);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void WpfConfigurator_OnSaveRequested(ConfigGroup configGroup)
        {
            try
            {
                DialogResult = true;
                ConfigManager.Save(configGroup);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void WpfConfigurator_OnCancelRequested()
        {
            try
            {
                DialogResult = false;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void OpenConfigWindow(string displayName, Type targetType)
        {
            try
            {
                var configWindow = new DefaultConfiguratorWindow(displayName, targetType)
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Width = 550,
                    Height = 600
                };
                configWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static bool? OpenConfigWindowDialog(string displayName, Type targetType)
        {
            try
            {
                var configWindow = new DefaultConfiguratorWindow(displayName, targetType)
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Width = 550,
                    Height = 600
                };
                
                return configWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
