using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
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

namespace WpfConfiguratorLib.view.editors
{
    /// <summary>
    /// Interaction logic for PasswordEditor.xaml
    /// </summary>
    public partial class PasswordEditor : UserControl
    {
        public static DependencyProperty LabelWidthPercentageProperty = DependencyProperty.Register(
            "LabelWidthPercentage", typeof(double), typeof(PasswordEditor), new PropertyMetadata((double)20));

        public double LabelWidthPercentage
        {
            get { return (double)GetValue(LabelWidthPercentageProperty); }
            set { SetValue(LabelWidthPercentageProperty, value); }
        }

        private ConfigPropertyInfo property { get { return DataContext as ConfigPropertyInfo; } }

        public PasswordEditor()
        {
            DataContextChanged += OnDataContextChanged;
            InitializeComponent();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            try
            {
                if (DataContext == null || property.Value == null) return;
                Password.Password = ConvertToUnsecureString(property.Value as SecureString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void Password_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                property.Value = Password.SecurePassword;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null) return "";

            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        private SecureString ConvertToSecureString(string password)
        {
            if (string.IsNullOrEmpty(password)) return new SecureString();

            unsafe
            {
                fixed (char* passwordChars = password)
                {
                    var securePassword = new SecureString(passwordChars, password.Length);
                    securePassword.MakeReadOnly();
                    return securePassword;
                }
            }
        }
    }
}
