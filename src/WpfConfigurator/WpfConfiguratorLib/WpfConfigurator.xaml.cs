using System;
using System.Windows;
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
            InitializeComponent();
        }



        #region Private Methods

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
