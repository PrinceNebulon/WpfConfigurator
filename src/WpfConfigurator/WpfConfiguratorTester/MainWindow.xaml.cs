using System;
using System.Windows;
using WpfConfiguratorLib;
using WpfConfiguratorLib.entities;

namespace WpfConfiguratorTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            WpfConfigurator.DataContext = ConfigManager.Load<TestData>("Test Data") ?? new TestData();
        }

        private void WpfConfigurator_OnSaveRequested(ConfigGroup configGroup)
        {
            try
            {
                ConfigManager.Save(configGroup);
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
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
