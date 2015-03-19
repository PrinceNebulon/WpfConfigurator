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
using WpfConfiguratorLib.view.editors.helpers;

namespace WpfConfiguratorLib.view
{
    /// <summary>
    /// Interaction logic for DefaultConfiguratorWindow.xaml
    /// </summary>
    public partial class DefaultConfiguratorWindow : Window
    {
        public ConfiguratorWindowResult Result = ConfiguratorWindowResult.None;

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
                Result = ConfiguratorWindowResult.Save;
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
                Result = ConfiguratorWindowResult.Cancel;
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

        public static ConfiguratorWindowResult OpenConfigWindowDialog(string displayName, Type targetType)
        {
            try
            {
                // Create window
                var configWindow = new DefaultConfiguratorWindow(displayName, targetType)
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Width = 550,
                    Height = 600
                };
                
                // Show window
                configWindow.ShowDialog();

                // Return result
                return configWindow.Result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return ConfiguratorWindowResult.None;
            }
        }
    }
}
