using System;
using System.Collections.Generic;
using System.Configuration;
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
using WpfConfiguratorLib.entities;

namespace WpfConfiguratorLib
{
    public partial class WpfConfigurator
    {
        #region Private Fields



        #endregion



        #region Public Properties

        public delegate void SaveRequestedHandler(ConfigGroup configGroup);

        public event SaveRequestedHandler SaveRequested;

        #endregion



        public WpfConfigurator()
        {
            //DataContextChanged += OnDataContextChanged;

            InitializeComponent();
        }



        #region Private Methods

        //private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        //{
        //    try
        //    {
        //        GenerateMembers();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //private void GenerateMembers()
        //{
        //    try
        //    {
        //        // Check config data
        //        var config =
        //            DynamicConfigurationElement.FromIDynamicConfigurationElement(
        //                DataContext as IDynamicConfigurationElement, "Root","Configuration base");
        //        if (config == null) return;

        //        Process(config, 0);

                
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //private void Process(DynamicConfigurationElement config, int level)
        //{
        //    foreach (var property in config.Properties)
        //    {
        //        Console.WriteLine("{0}Property {1} is {2} with value {3}", "".PadLeft(level, '>'), property.ConfigurationProperty.Name,
        //            property.ConfigurationProperty.Type, property.Value);
        //    }

        //    foreach (var child in config.Children)
        //    {
        //        Process(child, level + 1);
        //    }
        //}

        private void RaiseSaveRequested(ConfigGroup configGroup)
        {
            try
            {
                if (SaveRequested != null && configGroup != null) SaveRequested(configGroup);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion



        #region Public Methods



        #endregion

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                RaiseSaveRequested(DataContext as ConfigGroup);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
