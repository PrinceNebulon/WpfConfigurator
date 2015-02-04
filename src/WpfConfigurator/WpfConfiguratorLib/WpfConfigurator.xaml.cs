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

        public delegate void CancelRequestedHandler();

        public event CancelRequestedHandler CancelRequested;

        public static DependencyProperty LabelWidthPercentageProperty = DependencyProperty.Register(
            "LabelWidthPercentage", typeof(double), typeof(WpfConfigurator), new PropertyMetadata((double)20));

        public double LabelWidthPercentage
        {
            get { return (double)GetValue(LabelWidthPercentageProperty); }
            set { SetValue(LabelWidthPercentageProperty, value); }
        }

        public static readonly DependencyProperty ContentMinWidthProperty = DependencyProperty.Register(
            "ContentMinWidth", typeof (int), typeof (WpfConfigurator), new PropertyMetadata(100));

        public int ContentMinWidth
        {
            get { return (int) GetValue(ContentMinWidthProperty); }
            set { SetValue(ContentMinWidthProperty, value); }
        }

        public static readonly DependencyProperty ContentMaxWidthProperty = DependencyProperty.Register(
            "ContentMaxWidth", typeof (int), typeof (WpfConfigurator), new PropertyMetadata(Int32.MaxValue));

        public int ContentMaxWidth
        {
            get { return (int) GetValue(ContentMaxWidthProperty); }
            set { SetValue(ContentMaxWidthProperty, value); }
        }

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

        private void RaiseCancelRequested()
        {
            try
            {
                if (CancelRequested != null) CancelRequested();
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
                RaiseCancelRequested();
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
